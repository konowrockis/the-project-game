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
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class GameMessageHandlerTests
    {
        [TestMethod]
        public void Pass_GameMessage_to_GameMaster()
        {
            string playerGuid = "guid";
            ulong gameId = 1u;
            IClient client = Substitute.For<IClient>();
            client.PlayerGuid.Returns(playerGuid);
            IGame game = Substitute.For<IGame>();
            game.GameMaster.Returns(client);
            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            gamesManager.GetGameById(Arg.Any<ulong>()).Returns(game);
            GameMessage response = null;
            client.When(c => c.Write(Arg.Any<GameMessage>())).Do(c => response = c.Arg<GameMessage>());
            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);
            GameMessage message = new PickUpPiece();
            message.PlayerGuid = playerGuid;
            message.GameId = gameId;

            new GameMessageHandler(gamesManager, currentClient).Handle(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(message==response);
        }

        [TestMethod]
        public void Do_nothing_on_invalid_player_guid_in_GameMessage()
        {
            string playerGuid = "guid";
            ulong gameId = 1u;
            IClient client = Substitute.For<IClient>();
            client.PlayerGuid.Returns(playerGuid);
            IGame game = Substitute.For<IGame>();
            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            gamesManager.GetGamesList().Returns(new List<IGame>() { game });
            gamesManager.GetGameById(gameId).Returns(game);
            GameMessage response = null;
            client.When(c => c.Write(Arg.Any<GameMessage>())).Do(c => response = c.Arg<GameMessage>());
            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            currentClient.Value.Returns(c => client);
            GameMessage message = new PickUpPiece();
            message.PlayerGuid = playerGuid;
            message.GameId = gameId;
            
            new GameMessageHandler(gamesManager,currentClient).Handle(message);
            
            Assert.IsNull(response);
        }

    }
}
