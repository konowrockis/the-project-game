using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;
using TheProjectGame.Player.MessageHandlers;

namespace TheProjectGame.Player.Tests
{
    [TestClass]
    public class AcceptKnowledgeExchangeMessageHandlerTests
    {
        [TestMethod]
        public void Send_DataMessage_after_receiving_AcceptExchangeRequestMessage()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            IPlayerKnowledge knowledge = Substitute.For<IPlayerKnowledge>();
            var handler = new AcceptKnowlegdeExchangeMessageHandler(writer, knowledge);
            var message = new AcceptExchangeRequestMessage();

            handler.Handle(message);

            writer.Received().Write(Arg.Any<DataMessage>());
        }
    }
}
