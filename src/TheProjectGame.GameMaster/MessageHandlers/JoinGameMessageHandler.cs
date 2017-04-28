using System.Linq;
using AutoMapper;
using Serilog;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class JoinGameMessageHandler : MessageHandler<JoinGameMessage>
    {
        private ILogger logger = Log.ForContext<JoinGameMessageHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly IGameState game;
        private readonly IPlayersMap players;
        private readonly GameOptions gameOptions;
        private readonly IMapper mapper;
        private readonly ICurrentGame currentGame;

        public JoinGameMessageHandler(
            IMessageWriter messageWriter, 
            ICurrentGame currentGame, 
            GameMasterOptions gameOptions,
            IMapper mapper)
        {
            this.messageWriter = messageWriter;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.gameOptions = gameOptions.GameDefinition;
            this.mapper = mapper;
            this.currentGame = currentGame;
        }

        public override void Handle(JoinGameMessage message)
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
                var response = new RejectJoiningGameMessage()
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
            var response = new ConfirmJoiningGameMessage()
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
                // TODO: add proper Goal count
                game.Board.Init(game.Players, gameOptions.InitialNumberOfPieces, 1);

                var gameResponse = new GameStartedMessage()
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
                    logger.Verbose("Sending GameMessage to {@Player}", currentPlayer);

                    var response = new GameStartedMessage()
                    {
                        Board = gameResponse.Board,
                        Players = gameResponse.Players,
                        PlayerId = currentPlayer.Id,
                        PlayerLocation = mapper.Map<Location>(currentPlayer.Position)
                    };

                    messageWriter.Write(response);
                }
            }
        }
    }
}
