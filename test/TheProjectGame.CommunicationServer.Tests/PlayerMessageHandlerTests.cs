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

        private IClient player;
        private PlayerMessageHandler messageHandler;

        public PlayerMessageHandlerTests()
        {
            player = Substitute.For<IClient>();

            IClientsManager clientsManager = Substitute.For<IClientsManager>();

            clientsManager.GetPlayerById(playerId).Returns(player);
            clientsManager.GetPlayerById(nonExistentPlayerId).ReturnsNull();

            messageHandler = new PlayerMessageHandler(clientsManager);
        }

        [TestMethod]
        public void Pass_message_to_another_player_when_valid_playerId()
        {
            var message = GetMessage();

            messageHandler.Handle(message);

            player.Received().Write(message);
        }

        [TestMethod]
        public void Do_not_throw_exception_when_no_player_with_given_id()
        {
            var message = GetMessage();

            messageHandler.Handle(message);

            player.DidNotReceiveWithAnyArgs();
        }

        private PlayerMessage GetMessage(ulong playerId = playerId)
        {
            return new RejectJoiningGameMessage()
            {
                PlayerId = playerId
            };
        }
    }
}
