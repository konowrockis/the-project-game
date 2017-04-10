using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class MoveMessageHandler : MessageHandler<Move>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly GameState game;
        private readonly PlayersMap players;

        public MoveMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, GameState game, PlayersMap players)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = game;
            this.players = players;
        }

        public override void Handle(Move message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            var direction = message.Direction;

            logger.GameEvent(GameEvent.CreateFromMessage(message, player));

            var position = player.Position;
            var destination = position.Move(direction);
            var moveStatus = CheckMove(destination, board);

            var builder = new DataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            switch (moveStatus)
            {
                case MoveStatus.Valid:
                    board.MovePlayer(player, destination);
                    builder.PlayerLocation(destination.ToLocation())
                           .Fields(board, board.Fields[destination.X, destination.Y]);
                    break;
                case MoveStatus.Invalid:
                    builder.PlayerLocation(position.ToLocation());                    
                    break;
                case MoveStatus.Occupied:
                    builder.PlayerLocation(position.ToLocation())
                           .Fields(board, board.Fields[destination.X, destination.Y]);
                    break;
            }

            messageWriter.Write(builder.Build(), actionCosts.MoveDelay);
        }

        private MoveStatus CheckMove(Position destination, Board board)
        {
            if (!board.IsValid(destination)) return MoveStatus.Invalid;
            if (board.IsOccupied(destination)) return MoveStatus.Occupied;
            return MoveStatus.Valid;
        }

        private enum MoveStatus
        {
            Valid,
            Occupied,
            Invalid
        }
    }



  
}
