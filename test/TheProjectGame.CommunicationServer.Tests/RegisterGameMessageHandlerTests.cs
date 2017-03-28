using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class RegisterGameMessageHandlerTests
    {

        [TestMethod]
        public void Rejest_any_RegisterGame_message()
        {
            RejectGameRegistration response = null;
            IClient client = Substitute.For<IClient>();
            client.When(c => c.Write(Arg.Any<RejectGameRegistration>()))
                .Do(callback => response = callback.Arg<RejectGameRegistration>());
            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);
            RegisterGame message = new RegisterGame {NewGameInfo = new GameInfo {Name = "name"}};

            new RegisterGameMessageHandler(currentClient, null).Handle(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(message.NewGameInfo.Name == response.GameName);
        }

    }
}
