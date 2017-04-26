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

        private IClient player;
        private IClient gameMaster;
        private GameMessageHandler messageHandler;

        public GameMessageHandlerTests()
        {
            player = Substitute.For<IClient>();
            gameMaster = Substitute.For<IClient>();

            var currentClient = Substitute.For<ICurrentClient>();
            var gamesManager = Substitute.For<IGamesManager>();
            var game = Substitute.For<IGame>();

            currentClient.Value.Returns(player);
            gamesManager.GetGameById(gameId).Returns(game);
            gamesManager.GetGameById(nonExistentGameId).ReturnsNull();
            game.GameMaster.Returns(gameMaster);

            messageHandler = new GameMessageHandler(gamesManager, currentClient);
        }

        [TestMethod]
        public void Pass_GameMessage_to_GameMaster_when_everything_is_valid()
        {
            var message = GetMessage(validPlayerGuid);

            messageHandler.Handle(message);

            gameMaster.Received().Write(message);
        }

        [TestMethod]
        public void Do_nothing_on_invalid_player_guid()
        {
            var message = GetMessage(invalidPlayerGuid);

            messageHandler.Handle(message);

            gameMaster.DidNotReceiveWithAnyArgs();
        }

        [TestMethod]
        public void Do_nothing_when_game_with_given_id_does_not_exist()
        {
            var message = GetMessage(validPlayerGuid, nonExistentGameId);

            messageHandler.Handle(message);

            gameMaster.DidNotReceiveWithAnyArgs();
        }

        private GameMessage GetMessage(string playerGuid, ulong gameId = gameId)
        {
            return new TestPieceMessage()
            {
                PlayerGuid = playerGuid,
                GameId = gameId
            };
        }
    }
}
