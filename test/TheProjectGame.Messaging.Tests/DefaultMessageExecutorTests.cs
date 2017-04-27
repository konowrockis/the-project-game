using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts;
using TheProjectGame.Messaging.Default;

namespace TheProjectGame.Messaging.Tests
{
    [TestClass]
    public class DefaultMessageExecutorTests
    {
        private readonly IMessageHandlerResolver handlerResolver;
        private readonly IList<IMessageHandler> oneHandlerMessageHandlers;
        private readonly IList<IMessageHandler> multipleHandlersMessageHandlers;
        private readonly DefaultMessageExecutor executor;

        public DefaultMessageExecutorTests()
        {
            oneHandlerMessageHandlers = new List<IMessageHandler>
            {
                Substitute.For<IMessageHandler>()
            };

            multipleHandlersMessageHandlers = new List<IMessageHandler>
            {
                Substitute.For<IMessageHandler>(),
                Substitute.For<IMessageHandler>(),
                Substitute.For<IMessageHandler>()
            };

            handlerResolver = Substitute.For<IMessageHandlerResolver>();

            handlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Returns(oneHandlerMessageHandlers);
            handlerResolver.Resolve(Arg.Is(typeof(MultipleHandlersMessage))).Returns(multipleHandlersMessageHandlers);

            executor = new DefaultMessageExecutor(handlerResolver);
        }

        [TestMethod]
        public void Trying_to_execute_message_without_handlers_doesnt_throw_errors()
        {
            var message = new NoHandlersMessage();

            executor.Execute(message);

            handlerResolver.Resolve(Arg.Is(typeof(NoHandlersMessage))).Received();
        }

        [TestMethod]
        public void Trying_to_execute_message_without_handlers_doesnt_execute_any_handlers()
        {
            var message = new NoHandlersMessage();

            executor.Execute(message);

            foreach(var handler in oneHandlerMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
            foreach (var handler in multipleHandlersMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
        }

        [TestMethod]
        public void Executing_message_with_one_handler_executes_that_handler()
        {
            var message = new OneHandlerMessage();

            executor.Execute(message);

            handlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Received();
            foreach (var handler in oneHandlerMessageHandlers)
            {
                handler.Received().Handle(Arg.Is(message));
            }
            foreach (var handler in multipleHandlersMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
        }

        [TestMethod]
        public void Executing_message_with_multiple_handlers_executes_every_handler()
        {
            var message = new MultipleHandlersMessage();

            executor.Execute(message);

            handlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Received();
            foreach (var handler in oneHandlerMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
            foreach (var handler in multipleHandlersMessageHandlers)
            {
                handler.Received().Handle(Arg.Is(message));
            }
        }

        public class NoHandlersMessage : IMessage { }
        public class OneHandlerMessage : IMessage { }
        public class MultipleHandlersMessage : IMessage { }
    }
}
