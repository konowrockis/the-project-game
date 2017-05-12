using System;
using System.Linq;
using Serilog;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class PlacePieceMessageHandler : MessageHandler<PlacePieceMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly IGameState game;
        private readonly IPlayersMap players;
        private readonly GameMasterOptions options;
        private readonly Func<DataBuilder> dataBuilder;
        private readonly ActionCostsOptions actionCosts;
        private readonly ICurrentGame currentGame;

        public PlacePieceMessageHandler(
            IMessageWriter messageWriter,
            GameMasterOptions gameMasterOptions,
            ICurrentGame currentGame,
            Func<DataBuilder> dataBuilder)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = gameMasterOptions.ActionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.options = gameMasterOptions;
            this.dataBuilder = dataBuilder;
            this.currentGame = currentGame;
        }

        public override void Handle(PlacePieceMessage message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            var position = player.Position;

            var builder = dataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            var piece = board.Pieces.FirstOrDefault(p => p.Player == player);

            if (!board.IsInGoalArea(player.Position) && piece != null)
            {
                var taskTile = board.Fields[position.X, position.Y] as TaskTile;
                if (taskTile.Piece != null)
                {
                    // pass
                } else
                {
                    piece.Player = null;
                    taskTile.Piece = piece;
                    board.RefreshBoardState();
                }
                builder.Pieces(false, piece);
                builder.Fields(taskTile);
                messageWriter.Write(builder.Build(), actionCosts.PlacingDelay);
            }

            if (piece != null)
            {
                board.Pieces.Remove(piece);
                board.PlaceNewPiece();
                piece.Player = null;
                builder.Pieces(false,piece);
            }

            if (piece == null || piece.Type == PieceType.Sham)
            {
                messageWriter.Write(builder.Build(), actionCosts.PlacingDelay);
                return;
            }

            var goalTile = board.Fields[player.Position.X, player.Position.Y] as GoalTile;
            goalTile.Discovered = true;

            if (board.CheckWinConditions(player.Team))
            {
                foreach (var gamePlayer in game.Players)
                {
                    logger.GameEvent(gamePlayer.Team == player.Team
                        ? GameEvent.CreateVictory(message.PlayerGuid, game.Id, player.Id, player.Team, player.Role)
                        : GameEvent.CreateDefeat(message.PlayerGuid, game.Id, player.Id, player.Team, player.Role));

                    messageWriter.Write(new DataMessage()
                    {
                        GameFinished = true,
                        PlayerId = gamePlayer.Id
                    });
                }
                var registerGame = new RegisterGameMessage()
                {
                    NewGameInfo = new GameInfo()
                    {
                        Name = options.GameDefinition.GameName,
                        BlueTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam,
                        RedTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam
                    }
                };
                messageWriter.Write(registerGame);
            }
            else
            {
                var response = builder.Fields(goalTile).Build();
                messageWriter.Write(response, actionCosts.PlacingDelay);
            }
        }
    }
}
