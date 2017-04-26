using System;
using System.Linq;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class DiscoverMessageHandler : MessageHandler<DiscoverMessage>
    {
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;
        private readonly Func<DataBuilder> dataBuilder;

        public DiscoverMessageHandler(
            IMessageWriter messageWriter,
            GameMasterOptions gameMasterOptions,
            ICurrentGame currentGame,
            Func<DataBuilder> dataBuilder)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = gameMasterOptions.ActionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.dataBuilder = dataBuilder;
        }

        public override void Handle(DiscoverMessage message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            if (player == null) return;

            var tiles = board.GetNeighbourhood(player.Position.X, player.Position.Y).ToList();
            var response = dataBuilder()
                .Fields(tiles.ToArray())
                .GameFinished(false)
                .PlayerId(player.Id)
                .PlayerLocation(player.Position)
                .Build();

            messageWriter.Write(response, actionCosts.DiscoverDelay);
        }
    }
}
