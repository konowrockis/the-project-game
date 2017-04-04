using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.GameMaster.MessageHandlers;
using TheProjectGame.Messaging;

namespace TheProjectGame.GameMaster.Tests
{
    [TestClass]
    public class RejectGameRegistrationMessageHandlerTests
    {
        [TestMethod]
        public void Resend_RegisterGame_message_after_receiving_RejectGameRegistration_message()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            var handler = new RejectGameRegistrationMessageHandler(writer);
            var message = new RejectGameRegistration();

            handler.Handle(message);

            writer.Received().Write(Arg.Any<RegisterGame>(), 100);
        }
    }
}
