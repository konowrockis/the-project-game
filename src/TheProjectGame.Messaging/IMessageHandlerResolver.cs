using System;
using System.Collections.Generic;

namespace TheProjectGame.Messaging
{
    interface IMessageHandlerResolver
    {
        IList<IMessageHandler> Resolve(Type messageType);
    }
}
