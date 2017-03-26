
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging.Tests
{
    [TestClass]
    public class AutofacMessageHandlerResolverTests
    {
        [TestMethod]
        public void Resolving_not_existing_message_handler_returns_empty_array()
        {
            var resolver = GetMessageHandlerResolver();

            var handlers = resolver.Resolve(typeof(NoHandlersMessage));

            Assert.AreEqual(0, handlers.Count);
        }

        [TestMethod]
        public void Resolving_one_handler_message_returns_array_of_one_handler()
        {
            var resolver = GetMessageHandlerResolver();

            var handlers = resolver.Resolve(typeof(OneHandlerMessage));

            Assert.AreEqual(1, handlers.Count);
            Assert.IsInstanceOfType(handlers.First(), typeof(IMessageHandler<OneHandlerMessage>));
        }

        [TestMethod]
        public void Resolving_multiple_handler_message_returns_array_of_handlers()
        {
            var resolver = GetMessageHandlerResolver();

            var handlers = resolver.Resolve(typeof(MultipleHandlersMessage));

            Assert.AreEqual(2, handlers.Count);
            Assert.IsInstanceOfType(handlers.First(), typeof(IMessageHandler<MultipleHandlersMessage>));
        }

        private AutofacMessageHandlerResolver GetMessageHandlerResolver()
        {
            ILifetimeScope scope = Substitute.For<ILifetimeScope>();
            IComponentRegistry registry = Substitute.For<IComponentRegistry>();
            Guid oneHandlerMessageRegistrationId = Guid.NewGuid();
            Guid multipleHandlersMessageRegistrationId = Guid.NewGuid();
            IComponentRegistration componentRegistration = null;

            var oneHandlerMessageHandlers = new List<IMessageHandler<OneHandlerMessage>> {
                Substitute.For<IMessageHandler<OneHandlerMessage>>()
            };

            var multipleHandlersMessageHandlers = new List<IMessageHandler<MultipleHandlersMessage>> {
                Substitute.For<IMessageHandler<MultipleHandlersMessage>>(),
                Substitute.For<IMessageHandler<MultipleHandlersMessage>>()
            };

            scope.ResolveComponent(
                Arg.Is<IComponentRegistration>(cr => cr.Id == oneHandlerMessageRegistrationId),
                Arg.Any<IEnumerable<Parameter>>()).Returns(oneHandlerMessageHandlers);

            scope.ResolveComponent(
                Arg.Is<IComponentRegistration>(cr => cr.Id == multipleHandlersMessageRegistrationId),
                Arg.Any<IEnumerable<Parameter>>()).Returns(multipleHandlersMessageHandlers);

            scope.ComponentRegistry.Returns(registry);
            
            registry.TryGetRegistration(Arg.Is<TypedService>(s => s.ServiceType == typeof(IEnumerable<IMessageHandler<OneHandlerMessage>>)),
                out componentRegistration).Returns(x =>
                {
                    var registration = Substitute.For<IComponentRegistration>();
                    registration.Id.Returns(oneHandlerMessageRegistrationId);
                    x[1] = registration;
                    return true;
                });

            registry.TryGetRegistration(Arg.Is<TypedService>(s => s.ServiceType == typeof(IEnumerable<IMessageHandler<MultipleHandlersMessage>>)),
                out componentRegistration).Returns(x =>
                {
                    var registration = Substitute.For<IComponentRegistration>();
                    registration.Id.Returns(multipleHandlersMessageRegistrationId);
                    x[1] = registration;
                    return true;
                });

            return new AutofacMessageHandlerResolver(scope);
        }

        public class NoHandlersMessage : IMessage { }
        public class OneHandlerMessage : IMessage { }
        public class MultipleHandlersMessage : IMessage { }
    }
}
