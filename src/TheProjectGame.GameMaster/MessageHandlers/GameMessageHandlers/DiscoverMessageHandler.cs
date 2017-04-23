using System.Linq;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class DiscoverMessageHandler : MessageHandler<Discover>
    {
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;

        public DiscoverMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
        }

        public override void Handle(Discover message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            if (player == null) return;

            var tiles = board.GetNeighbourhood(player.Position.X, player.Position.Y).ToList();
            var response = new DataBuilder()
                .Fields(tiles.ToArray())
                .GameFinished(false)
                .PlayerId(player.Id)
                .PlayerLocation(ObjectMapper.Map(player.Position))
                .Build();
            messageWriter.Write(response, actionCosts.DiscoverDelay);
        }
    }
}
