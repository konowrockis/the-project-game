using System.Linq;
using Serilog;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers.GameMessageHandlers
{
    class TestPieceMessageHandler : MessageHandler<TestPiece>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;

        public TestPieceMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
        }

        public override void Handle(TestPiece message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);

            var builder = new DataBuilder()
                .GameFinished(false)
                .PlayerLocation(ObjectMapper.Map(player.Position))
                .PlayerId(player.Id);

            // find piece
            var piece = board.Pieces.FirstOrDefault(p => p.Player == player);
            // is player doesnt carry return empty message
            if (piece == null)
            {
                messageWriter.Write(builder.Build(),actionCosts.TestDelay);
                return;
            }
            // is piece is a sham remove it from the game
            // and add a new piece
            if (piece.Type == PieceType.Sham)
            {
                board.Pieces.Remove(piece);
                board.PlaceNewPiece();
            }
            board.RefreshBoardState();
            // return information about the piece
            var response = builder.Pieces(true, piece).Build();

            messageWriter.Write(response,actionCosts.TestDelay);
        }
    }
}
