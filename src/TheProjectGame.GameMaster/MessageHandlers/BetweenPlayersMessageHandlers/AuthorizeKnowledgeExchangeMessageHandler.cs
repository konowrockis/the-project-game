using Serilog;
using System.Linq;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;
using TheProjectGame.Game;
using TheProjectGame.Contracts;

namespace TheProjectGame.GameMaster.MessageHandlers.BetweenPlayersMessageHandlers
{
    class AuthorizeKnowledgeExchangeMessageHandler : MessageHandler<AuthorizeKnowledgeExchange>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly ICurrentGame currentGame;

        public AuthorizeKnowledgeExchangeMessageHandler(
            IMessageWriter messageWriter,
            GameMasterOptions gameMasterOptions,
            ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.currentGame = currentGame;
            this.actionCosts = gameMasterOptions.ActionCosts;
        }

        public override void Handle(AuthorizeKnowledgeExchange message)
        {
            var player = currentGame.Players.GetPlayer(message.PlayerGuid);
            if (player == null) return;

            var receiver = currentGame.Game.Players.FirstOrDefault(p => p.Id == message.WithPlayerId);

            IMessage response = receiver == null ? RejectKnowledgeExchange(message, player) : PassRequest(message, player);

            messageWriter.Write(response, actionCosts.KnowledgeExchangeDelay);
        }

        private IMessage RejectKnowledgeExchange(AuthorizeKnowledgeExchange message, GamePlayer player) =>
            new RejectKnowledgeExchange()
            {
                Permanent = true,
                PlayerId = player.Id,
                SenderPlayerId = message.WithPlayerId
            };

        private IMessage PassRequest(AuthorizeKnowledgeExchange message, GamePlayer player) =>
            new KnowledgeExchangeRequest()
            {
                PlayerId = message.WithPlayerId,
                SenderPlayerId = player.Id
            };
    }
}
