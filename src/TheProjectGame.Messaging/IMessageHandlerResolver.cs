using System;
using System.Collections.Generic;

namespace TheProjectGame.Messaging
{
    public interface IMessageHandlerResolver
    {
        IList<IMessageHandler> Resolve(Type messageType);
    }
}
