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
    class AcceptExchangeRequestMessageHandler : MessageHandler<AcceptExchangeRequest>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;

        public AcceptExchangeRequestMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts,
            ICurrentGame currentGame)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
        }
        public override void Handle(AcceptExchangeRequest message)
        {

            var response = new AcceptExchangeRequest
            {
                PlayerId = message.PlayerId,
                SenderPlayerId = message.SenderPlayerId
            };
            messageWriter.Write(response, actionCosts.KnowledgeExchangeDelay);
        }
    }
}
