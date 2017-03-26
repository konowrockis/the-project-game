
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
    public class DefaultMessageExecutorTests
    {
        [TestMethod]
        public void Trying_to_execute_message_without_handlers_doesnt_throw_errors()
        {
            SystemUnderTests sut;
            var executor = GetMessageExecutor(out sut);

            executor.Execute(new NoHandlersMessage());

            sut.HandlerResolver.Resolve(Arg.Is(typeof(NoHandlersMessage))).Received();
        }

        [TestMethod]
        public void Trying_to_execute_message_without_handlers_doesnt_execute_any_handlers()
        {
            SystemUnderTests sut;
            var executor = GetMessageExecutor(out sut);

            executor.Execute(new NoHandlersMessage());

            foreach(var handler in sut.OneHandlerMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
            foreach (var handler in sut.MultipleHandlersMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
        }

        [TestMethod]
        public void Executing_message_with_one_handler_executes_that_handler()
        {
            SystemUnderTests sut;
            var executor = GetMessageExecutor(out sut);
            var message = new OneHandlerMessage();

            executor.Execute(message);

            sut.HandlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Received();
            foreach (var handler in sut.OneHandlerMessageHandlers)
            {
                handler.Received().Handle(Arg.Is(message));
            }
            foreach (var handler in sut.MultipleHandlersMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
        }

        [TestMethod]
        public void Executing_message_with_multiple_handlers_executes_every_handler()
        {
            SystemUnderTests sut;
            var executor = GetMessageExecutor(out sut);
            var message = new MultipleHandlersMessage();

            executor.Execute(message);

            sut.HandlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Received();
            foreach (var handler in sut.OneHandlerMessageHandlers)
            {
                handler.DidNotReceiveWithAnyArgs().Handle(Arg.Any<IMessage>());
            }
            foreach (var handler in sut.MultipleHandlersMessageHandlers)
            {
                handler.Received().Handle(Arg.Is(message));
            }
        }

        private DefaultMessageExecutor GetMessageExecutor(out SystemUnderTests sut)
        {
            var oneHandlerMessageHandlers = new List<IMessageHandler>
            {
                Substitute.For<IMessageHandler>()
            };

            var multipleHandlersMessageHandlers = new List<IMessageHandler>
            {
                Substitute.For<IMessageHandler>(),
                Substitute.For<IMessageHandler>(),
                Substitute.For<IMessageHandler>()
            };

            sut = new SystemUnderTests()
            {
                HandlerResolver = Substitute.For<IMessageHandlerResolver>(),
                OneHandlerMessageHandlers = oneHandlerMessageHandlers,
                MultipleHandlersMessageHandlers = multipleHandlersMessageHandlers
            };

            sut.HandlerResolver.Resolve(Arg.Is(typeof(OneHandlerMessage))).Returns(sut.OneHandlerMessageHandlers);
            sut.HandlerResolver.Resolve(Arg.Is(typeof(MultipleHandlersMessage))).Returns(sut.MultipleHandlersMessageHandlers);

            return new DefaultMessageExecutor(sut.HandlerResolver);
        }

        public class NoHandlersMessage : IMessage { }
        public class OneHandlerMessage : IMessage { }
        public class MultipleHandlersMessage : IMessage { }

        private class SystemUnderTests
        {
            public IMessageHandlerResolver HandlerResolver { get; set; }
            public IList<IMessageHandler> OneHandlerMessageHandlers { get; set; }
            public IList<IMessageHandler> MultipleHandlersMessageHandlers { get; set; }
        }
    }
}
