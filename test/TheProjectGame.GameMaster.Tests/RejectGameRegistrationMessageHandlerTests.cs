using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void ResendRegisterGameMessage()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            RegisterGame response = null;
            writer.When(w=>w.Write(Arg.Any<RegisterGame>(),Arg.Any<double>())).Do(c=>response=c.Arg<RegisterGame>());

            new RejectGameRegistrationMessageHandler(writer).Handle(new RejectGameRegistration());
            Assert.IsNotNull(response);
        }
    }
}
