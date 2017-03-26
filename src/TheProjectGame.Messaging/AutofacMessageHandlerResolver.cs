using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace TheProjectGame.Messaging
{
    public class AutofacMessageHandlerResolver : IMessageHandlerResolver
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);
        private static readonly Type HandlerType = typeof(IMessageHandler<>);
        private readonly ILifetimeScope scope;

        public AutofacMessageHandlerResolver(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public IList<IMessageHandler> Resolve(Type messageType)
        {
            Type handlerType = HandlerType.MakeGenericType(messageType);
            Type enumerableHandlerType = EnumerableType.MakeGenericType(handlerType);

            object handlers;

            if (!scope.TryResolve(enumerableHandlerType, out handlers))
            {
                return new List<IMessageHandler>();
            }

            return ((IEnumerable<IMessageHandler>)handlers).ToList();
        }
    }
}
