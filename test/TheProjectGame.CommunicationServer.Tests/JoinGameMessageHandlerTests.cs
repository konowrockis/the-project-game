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
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class JoinGameMessageHandlerTests
    {
        

        [TestInitialize]
        public void TestInitialize()
        {
            
        }


        [TestMethod]
        public void NonExistentGameName()
        {
            IClient client = Substitute.For<IClient>();

            JoinGame joinGame = new JoinGame
            {
                GameName = "test",
                PreferedRole = PlayerType.Player,
                PreferedTeam = TeamColor.Blue,
                PlayerIdSpecified = false
            };

            bool wrote = false;

            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            gamesManager.GetGameByName(Arg.Any<string>()).ReturnsNull();
            
            client.When(c=>c.Write(Arg.Any<RejectJoiningGame>())).Do(c=>wrote=true);

            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c=>client);

            JoinGameMessageHandler handler = new JoinGameMessageHandler(currentClient,gamesManager);

            handler.Handle(joinGame);

            Assert.IsTrue(wrote);
        }

        [TestMethod]
        public void ValidGameName()
        {
            ulong playerId = 1u;

            IClient client = Substitute.For<IClient>();
            IGamesManager gamesManager = Substitute.For<IGamesManager>();

            IGame game = Substitute.For<IGame>();
            game.GameMaster.Returns(client);

            client.PlayerId.Returns(playerId);

            gamesManager.GetGameByName(Arg.Any<string>()).Returns(game);

            bool passed = false;

            client.When(c=>c.Write(Arg.Any<JoinGame>())).Do(c =>
            {
                passed = c.Arg<JoinGame>().PlayerId == playerId;
            });

            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);
            JoinGameMessageHandler handler = new JoinGameMessageHandler(currentClient, gamesManager);

            JoinGame joinGame = new JoinGame
            {
                GameName = "test",
                PreferedRole = PlayerType.Player,
                PreferedTeam = TeamColor.Blue,
                PlayerIdSpecified = false
            };
            handler.Handle(joinGame);

            Assert.IsTrue(passed);
        }

    }
}
