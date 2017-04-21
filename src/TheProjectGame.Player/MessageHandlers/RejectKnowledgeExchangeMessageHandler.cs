using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class RejectKnowledgeExchangeMessageHandler : MessageHandler<RejectKnowledgeExchange>
    {
        private readonly ILogger logger = Log.ForContext<PlayerEventHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;

        public RejectKnowledgeExchangeMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
        }
        public override void Handle(RejectKnowledgeExchange message)
        {

            var response = new RejectKnowledgeExchange
            {
                Permanent = message.Permanent,
                PlayerId = message.PlayerId,
                SenderPlayerId = message.SenderPlayerId
            };
            messageWriter.Write(response, actionCosts.KnowledgeExchangeDelay);
        }
    }
}
