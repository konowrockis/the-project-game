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
using static TheProjectGame.Game.Builders.ObjectMapper;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class MoveMessageHandler : MessageHandler<MoveMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;

        public MoveMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
        }

        public override void Handle(MoveMessage message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            var direction = message.Direction;
            var position = player.Position;
            var destination = position.Move(direction);
            var moveStatus = CheckMove(destination, board);

            var builder = new DataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            if (moveStatus == MoveStatus.Valid)
            {
                board.MovePlayer(player, destination);
            }
            board.RefreshBoardState();
            builder.PlayerLocation(Map(player.Position));
            if (moveStatus != MoveStatus.Invalid)
            {
                builder.Fields(board.Fields[destination.X, destination.Y]);
            }

            var response = builder.Build();
            messageWriter.Write(response, actionCosts.MoveDelay);
        }

        private MoveStatus CheckMove(Position destination, IBoard board)
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