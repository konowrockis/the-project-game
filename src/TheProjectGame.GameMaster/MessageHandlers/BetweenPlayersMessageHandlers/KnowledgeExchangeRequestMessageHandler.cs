using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers.BetweenPlayersMessageHandlers
{
    class KnowledgeExchangeRequestMessageHandler : MessageHandler<KnowledgeExchangeRequest>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;

        public KnowledgeExchangeRequestMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts,
            ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
        }
        public override void Handle(KnowledgeExchangeRequest message)
        {
            
            var response = new KnowledgeExchangeRequest
            {
                PlayerId = message.PlayerId,
                SenderPlayerId = message.SenderPlayerId
            };
            messageWriter.Write(response, actionCosts.KnowledgeExchangeDelay);
        }
    }
}
