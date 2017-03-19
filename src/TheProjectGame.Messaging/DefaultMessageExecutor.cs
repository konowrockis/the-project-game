using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    class DefaultMessageExecutor : IMessageExecutor
    {
        private readonly IMessageHandlerResolver messageHandlerResolver;

        public DefaultMessageExecutor(IMessageHandlerResolver messageHandlerResolver)
        {
            this.messageHandlerResolver = messageHandlerResolver;
        }

        public void Execute<TMessage>(TMessage message) where TMessage : IMessage
        {
            var handlers = messageHandlerResolver.Resolve(message.GetType());

            foreach (var handler in handlers)
            {
                handler.Handle(message);
            }
        }
    }
}
