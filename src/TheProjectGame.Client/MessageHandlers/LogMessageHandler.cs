using System;
using TheProjectGame.Contracts;
using TheProjectGame.Messaging;

namespace TheProjectGame.Client.MessageHandlers
{
    class LogMessageHandler : MessageHandler<IMessage>
    {
        public override void Handle(IMessage message)
        {
            Console.WriteLine("Received message.");
            // TODO: display message content when logging is working
        }
    }
}
