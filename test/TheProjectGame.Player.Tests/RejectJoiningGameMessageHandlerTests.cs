using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Player.MessageHandlers;

namespace TheProjectGame.Player.Tests
{
    [TestClass]
    public class RejectJoiningGameMessageHandlerTests
    {
        [TestMethod]
        public void Send_GetGames_message_after_receiving_RejectJoiningGame()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            var handler = new RejectJoiningGameMessageHandler(writer);
            var message = new RejectJoiningGame();

            handler.Handle(message);

            writer.Received().Write(Arg.Any<GetGames>());
        }
    }
}
