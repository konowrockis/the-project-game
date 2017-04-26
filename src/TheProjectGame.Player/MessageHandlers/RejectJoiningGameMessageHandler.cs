using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.Player.MessageHandlers
{
    class RejectJoiningGameMessageHandler : MessageHandler<RejectJoiningGameMessage>
    {
        private readonly IMessageWriter messageWriter;

        public RejectJoiningGameMessageHandler(IMessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public override void Handle(RejectJoiningGameMessage message)
        {
            messageWriter.Write(new GetGamesMessage());
        }
    }
}
