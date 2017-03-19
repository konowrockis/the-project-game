using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    class AutofacMessageHandlerResolver : IMessageHandlerResolver
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);
        private static readonly Type HandlerType = typeof(IMessageHandler<>);
        private readonly IComponentContext componentContext;

        public AutofacMessageHandlerResolver(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public IList<IMessageHandler> Resolve(Type messageType)
        {
            Type handlerType = HandlerType.MakeGenericType(messageType);
            Type enumerableHandlerType = EnumerableType.MakeGenericType(handlerType);

            object handlers;
            componentContext.TryResolve(enumerableHandlerType, out handlers);

            return ((IEnumerable<IMessageHandler>)handlers).ToList();
        }
    }
}
