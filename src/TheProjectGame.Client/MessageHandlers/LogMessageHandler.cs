using System;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Messaging;

namespace TheProjectGame.Client.MessageHandlers
{
    class LogMessageHandler : MessageHandler<IMessage>
    {
        private readonly ILogger logger = Log.ForContext<LogMessageHandler>();

        public override void Handle(IMessage message)
        {
            logger.Debug("Received message: " + message.GetType().ToString());
            //Console.WriteLine("Received message: " + message.GetType().ToString());
            // TODO: display message content when logging is working
        }
    }
}
