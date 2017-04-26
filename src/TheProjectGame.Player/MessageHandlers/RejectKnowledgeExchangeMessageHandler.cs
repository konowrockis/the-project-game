using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class RejectKnowledgeExchangeMessageHandler : MessageHandler<RejectKnowledgeExchangeMessage>
    {
        private readonly IMessageWriter messageWriter;
        private readonly IPlayerLogic playerLogic;
        private readonly IPlayerKnowledge playerKnowledge;

        public RejectKnowledgeExchangeMessageHandler(
            IMessageWriter messageWriter, 
            IPlayerLogic playerLogic,
            IPlayerKnowledge playerKnowledge)
        {
            this.messageWriter = messageWriter;
            this.playerLogic = playerLogic;
            this.playerKnowledge = playerKnowledge;
        }

        public override void Handle(RejectKnowledgeExchangeMessage message)
        {
            // TODO: Handle permanent flag

            var response = playerLogic.GetNextMove();

            messageWriter.Write(response);
        }
    }
}
