using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class PlayerMessageHandlerTests
    {

        [TestMethod]
        public void PlayerMessageTest()
        {
            bool passed = false;
            IClient player = Substitute.For<IClient>();
            player.When(c=>player.Write(Arg.Any<PlayerMessage>())).Do(callback=>passed=true);

            IClientsManager manager = Substitute.For<IClientsManager>();
            manager.GetPlayerById(Arg.Any<ulong>()).Returns(player);

            PlayerMessage message = new PlayerMessage()
            {
                PlayerId = 1u
            };

            new PlayerMessageHandler(manager).Handle(message);

            Assert.IsTrue(passed);
        }

    }
}
