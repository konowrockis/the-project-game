using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class GameMessageHandlerTests
    {
        private const ulong gameId = 1;
        private const ulong nonExistentGameId = gameId + 1;
        private const string validPlayerGuid = nameof(validPlayerGuid);
        private const string invalidPlayerGuid = nameof(invalidPlayerGuid);

        [TestMethod]
        public void Pass_GameMessage_to_GameMaster_when_everything_is_valid()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(validPlayerGuid);

            sut.MessageHandler.Handle(message);

            sut.GameMaster.Received().Write(message);
        }

        [TestMethod]
        public void Do_nothing_on_invalid_player_guid()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(invalidPlayerGuid);

            sut.MessageHandler.Handle(message);

            sut.GameMaster.DidNotReceiveWithAnyArgs();
        }

        [TestMethod]
        public void Do_nothing_when_game_with_given_id_does_not_exist()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(validPlayerGuid, nonExistentGameId);

            sut.MessageHandler.Handle(message);

            sut.GameMaster.DidNotReceiveWithAnyArgs();
        }

        private SystemUnderTests GetMessageHandler()
        {
            var client = Substitute.For<IClient>();
            var currentClient = Substitute.For<ICurrentClient>();
            var gamesManager = Substitute.For<IGamesManager>();
            var game = Substitute.For<IGame>();
            var gameMaster = Substitute.For<IClient>();

            currentClient.Value.Returns(client);
            gamesManager.GetGameById(gameId).Returns(game);
            gamesManager.GetGameById(nonExistentGameId).ReturnsNull();
            game.GameMaster.Returns(gameMaster);

            return new SystemUnderTests()
            {
                MessageHandler = new GameMessageHandler(gamesManager, currentClient),
                GameMaster = gameMaster,
                Client = client
            };
        }

        private GameMessage GetMessage(string playerGuid, ulong gameId = gameId)
        {
            return new TestPiece()
            {
                PlayerGuid = playerGuid,
                GameId = gameId
            };
        }

        private class SystemUnderTests
        {
            public GameMessageHandler MessageHandler { get; set; }
            public IClient GameMaster { get; set; }
            public IClient Client { get; set; }
        }
    }
}
