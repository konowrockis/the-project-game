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
    class AuthorizeKnowledgeExchangeMessageHandler : MessageHandler<AuthorizeKnowledgeExchangeMessage>
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

        public override void Handle(AuthorizeKnowledgeExchangeMessage message)
        {
            var player = currentGame.Players.GetPlayer(message.PlayerGuid);
            if (player == null) return;

            var receiver = currentGame.Game.Players.FirstOrDefault(p => p.Id == message.WithPlayerId);

            // TODO: implement
            IMessage response = true ? RejectKnowledgeExchange(message, player) : PassRequest(message, player);

            messageWriter.Write(response, actionCosts.KnowledgeExchangeDelay);
        }

        private IMessage RejectKnowledgeExchange(AuthorizeKnowledgeExchangeMessage message, GamePlayer player) =>
            new RejectKnowledgeExchangeMessage()
            {
                Permanent = true,
                PlayerId = player.Id,
                SenderPlayerId = message.WithPlayerId
            };

        private IMessage PassRequest(AuthorizeKnowledgeExchangeMessage message, GamePlayer player) =>
            new KnowledgeExchangeRequestMessage()
            {
                PlayerId = message.WithPlayerId,
                SenderPlayerId = player.Id
            };
    }
}
