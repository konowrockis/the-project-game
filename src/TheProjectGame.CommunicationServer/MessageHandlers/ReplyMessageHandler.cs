using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class ReplyMessageHandler : MessageHandler<IMessage>
    {
        private readonly IClient currentClient;

        public ReplyMessageHandler(ICurrentClient currentClient)
        {
            this.currentClient = currentClient.Value;
        }

        public override void Handle(IMessage message)
        {
            currentClient.Write(message);
        }
    }
}
