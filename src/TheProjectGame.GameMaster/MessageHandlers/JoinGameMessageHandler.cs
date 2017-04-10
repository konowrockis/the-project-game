using System.Linq;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class JoinGameMessageHandler : MessageHandler<JoinGame>
    {
        private readonly IMessageWriter messageWriter;
        private readonly GameState game;
        private readonly PlayersMap players;
        private readonly GameOptions gameOptions;

        public JoinGameMessageHandler(IMessageWriter messageWriter, ICurrentGame currentGame, GameMasterOptions gameOptions)
        {
            this.messageWriter = messageWriter;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.gameOptions = gameOptions.GameDefinition;
        }

        public override void Handle(JoinGame message)
        {
            GamePlayer player = new GamePlayer(message.PlayerId);

            if (HandleIfGameIsFull(message.GameName, player)) return;

            UpdatePlayerTeamAndRole(player, message.PreferedTeam, message.PreferedRole);

            var guid = AddPlayer(player);

            SendConfirmJoiningGame(player, guid);

            HandleIfGameReadyToStart();
        }

        private bool HandleIfGameIsFull(string gameName, GamePlayer player)
        {
            if (game.Players.Count >= gameOptions.NumberOfPlayersPerTeam * 2)
            {
                var response = new RejectJoiningGame()
                {
                    GameName = gameName,
                    PlayerId = player.Id
                };

                messageWriter.Write(response);

                return true;
            }

            return false;
        }

        private void UpdatePlayerTeamAndRole(GamePlayer player, TeamColor teamColor, PlayerType role)
        {
            var freeSlots = gameOptions.NumberOfPlayersPerTeam - game.TeamPlayers(teamColor).Count();

            if (freeSlots == 0)
            {
                teamColor = teamColor == TeamColor.Blue ? TeamColor.Red : TeamColor.Blue;
                freeSlots = game.Players.Count - game.TeamPlayers(teamColor).Count();
            }

            var hasLeader = game.TeamPlayers(teamColor).Any(p => p.Role == PlayerType.Leader);

            if (freeSlots == 1 && !hasLeader)
            {
                role = PlayerType.Leader;
            }
            else if (hasLeader)
            {
                role = PlayerType.Player;
            }

            player.InitTeam(teamColor, role);

        }

        private string AddPlayer(GamePlayer player)
        {
            game.Players.Add(player);

            return players.AddPlayer(player);
        }

        private void SendConfirmJoiningGame(GamePlayer player, string guid)
        {
            var response = new ConfirmJoiningGame()
            {
                PlayerId = player.Id,
                GameId = game.Id,
                PrivateGuid = guid,
                PlayerDefinition = new Player()
                {
                    Id = player.Id,
                    Team = player.Team,
                    Type = player.Role
                }
            };

            messageWriter.Write(response);
        }

        public void HandleIfGameReadyToStart()
        {
            if (game.Players.Count >= gameOptions.NumberOfPlayersPerTeam * 2)
            {
                game.Board.Init(game.Players,gameOptions.InitialNumberOfPieces);

                var gameResponse = new Contracts.Messages.GameActions.Game()
                {
                    Board = new GameBoard()
                    {
                        Width = game.Board.BoardWidth,
                        TasksHeight = game.Board.TaskAreaHeight,
                        GoalsHeight = game.Board.GoalAreaHeight
                    },
                    Players = game.Players.Select(p => new Player()
                    {
                        Id = p.Id,
                        Team = p.Team,
                        Type = p.Role
                    }).ToList(),
                };

                foreach (var currentPlayer in game.Players)
                {
                    gameResponse.PlayerId = currentPlayer.Id;
                    gameResponse.PlayerLocation = currentPlayer.Position.ToLocation();

                    messageWriter.Write(gameResponse);
                }
            }
        }
    }
}
