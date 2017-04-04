using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class PlayerMessageHandlerTests
    {
        private const ulong playerId = 1;
        private const ulong nonExistentPlayerId = playerId + 1;

        [TestMethod]
        public void Pass_message_to_another_player_when_valid_playerId()
        {
            var sut = GetMessageHandler();
            var message = GetMessage();

            sut.MessageHandler.Handle(message);

            sut.Player.Received().Write(message);
        }

        [TestMethod]
        public void Do_not_throw_exception_when_no_player_with_given_id()
        {
            var sut = GetMessageHandler();
            var message = GetMessage();

            sut.MessageHandler.Handle(message);

            sut.Player.DidNotReceiveWithAnyArgs();
        }

        private SystemUnderTests GetMessageHandler()
        {
            IClientsManager clientsManager = Substitute.For<IClientsManager>();
            IClient player = Substitute.For<IClient>();

            clientsManager.GetPlayerById(playerId).Returns(player);
            clientsManager.GetPlayerById(nonExistentPlayerId).ReturnsNull();

            return new SystemUnderTests()
            {
                MessageHandler = new PlayerMessageHandler(clientsManager),
                Player = player
            };
        }

        private PlayerMessage GetMessage(ulong playerId = playerId)
        {
            return new RejectJoiningGame()
            {
                PlayerId = playerId
            };
        }

        private class SystemUnderTests
        {
            public PlayerMessageHandler MessageHandler { get; set; }
            public IClient Player { get; set; }
        }
    }
}
