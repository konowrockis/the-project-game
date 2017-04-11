using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class PlacePieceMessageHandler : MessageHandler<PlacePiece>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly GameState game;
        private readonly PlayersMap players;

        public PlacePieceMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, GameState game, PlayersMap players)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = game;
            this.players = players;
        }

        public override void Handle(PlacePiece message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            
            logger.GameEvent(GameEvent.CreateFromMessage(message, player));

            var builder = new DataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            var piece = board.Pieces.FirstOrDefault(p => p.Player == player);
            if (piece != null)
            {
                board.Pieces.Remove(piece);
            }

            if (piece == null || !board.IsInGoalArea(player.Position) || piece.Type == PieceType.Sham)
            {
                messageWriter.Write(message, actionCosts.PlacingDelay);
                return;
            }
            
            var goalTile = board.Fields[player.Position.X, player.Position.Y] as GoalTile;
            if (goalTile.Type == GoalFieldType.Goal)
            {
                goalTile.Discovered = true;
            }

            var response = builder.Fields(board, goalTile).Build();
            messageWriter.Write(response,actionCosts.PlacingDelay);

            if (board.CheckWinConditions(player.Team))
            {
                logger.GameEvent(GameEvent.CreateVictory(message.PlayerGuid, game.Id, player.Id, player.Team, player.Role));
                foreach (var gamePlayer in game.Players)
                {
                    messageWriter.Write(new Data()
                    {
                        GameFinished = true,
                        PlayerId = gamePlayer.Id
                    });
                }
            }
        }
    }
}
