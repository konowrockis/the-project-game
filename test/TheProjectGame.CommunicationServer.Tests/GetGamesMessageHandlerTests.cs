using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class GetGamesMessageHandlerTests
    {

        [TestMethod]
        public void GetRegisteredGamesTest()
        {
            string gameName = "testName";
            IClient client = Substitute.For<IClient>();
            RegisteredGames response = null;

            IGame game = Substitute.For<IGame>();
            game.Name.Returns(gameName);

            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            gamesManager.GetGamesList().Returns(new List<IGame>() {game});

            client.When(c => c.Write(Arg.Any<RegisteredGames>())).Do(c => response = c.Arg<RegisteredGames>());

            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);

            new GetGamesMessageHandler(currentClient,gamesManager).Handle(new GetGames());

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.GameInfo);
            Assert.IsTrue(response.GameInfo.Count==1);
            Assert.IsTrue(response.GameInfo[0].Name==gameName);
        }


        [TestMethod]
        public void NoGamesRegisteredTest()
        {
            IClient client = Substitute.For<IClient>();
            RegisteredGames response = null;

            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            gamesManager.GetGamesList().Returns(new List<IGame>());

            client.When(c => c.Write(Arg.Any<RegisteredGames>())).Do(c => response = c.Arg<RegisteredGames>());

            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);

            new GetGamesMessageHandler(currentClient, gamesManager).Handle(new GetGames());

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.GameInfo);
            Assert.IsTrue(response.GameInfo.Count==0);
        }
    }
}
