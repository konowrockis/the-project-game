using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers.GameMessageHandlers
{
    class PickupPieceMessageHandler : MessageHandler<PickUpPieceMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;

        public PickupPieceMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts,
            ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
        }

        public override void Handle(PickUpPieceMessage message)
        {
            var board = game.Board;
            var gamePlayer = players.GetPlayer(message.PlayerGuid);
            var position = gamePlayer.Position;
            if (gamePlayer == null) return;
            var tile = board.Fields[position.X, position.Y];

            if (board.IsInGoalArea(position))
            {
                messageWriter.Write(EmptyData(tile,gamePlayer),actionCosts.PickUpDelay);
                return;
            }

            var field = board.Fields[position.X, position.Y] as TaskTile;
            if (field.Piece == null)
            {
                messageWriter.Write(EmptyData(tile, gamePlayer), actionCosts.PickUpDelay);
                return;
            }


            var piece = field.Piece;
            piece.Player = gamePlayer;
            field.Piece = null;

            DataBuilder builder = new DataBuilder();
            var response = builder.GameFinished(false)
                .PlayerId(gamePlayer.Id)
                .PlayerLocation(ObjectMapper.Map(gamePlayer.Position))
                .Pieces(piece)
                .Fields(field)
                .Build();

            var responsePieces = response.Pieces;

            messageWriter.Write(response,actionCosts.PickUpDelay);
        }

        public IMessage EmptyData(Tile tile, GamePlayer player)
        {
            return new DataBuilder().GameFinished(false)
                .PlayerId(player.Id)
                .PlayerLocation(ObjectMapper.Map(player.Position))
                .Fields(tile)
                .Build();
        }

    }
}
