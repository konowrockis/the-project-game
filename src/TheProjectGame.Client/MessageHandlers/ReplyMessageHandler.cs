using TheProjectGame.Contracts;
using TheProjectGame.Messaging;

namespace TheProjectGame.Client.MessageHandlers
{
    class ReplyMessageHandler : MessageHandler<IMessage>
    {
        private readonly IMessageWriter messageWriter;

        public ReplyMessageHandler(IMessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public override void Handle(IMessage message)
        {
            messageWriter.Write(message);
        }
    }
}
