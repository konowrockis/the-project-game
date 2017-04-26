using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class JoinGameMessageHandlerTests
    {
        private const ulong playerId = 1;
        private const string existingGameName = nameof(existingGameName);
        private const string nonExistingGameName = nameof(nonExistingGameName);

        private IClient gameMaster;
        private IClient client;
        private JoinGameMessageHandler messageHandler;

        public JoinGameMessageHandlerTests()
        {
            client = Substitute.For<IClient>();
            gameMaster = Substitute.For<IClient>();

            var currentClient = Substitute.For<ICurrentClient>();
            var gamesManager = Substitute.For<IGamesManager>();
            var game = Substitute.For<IGame>();

            client.PlayerId.Returns(playerId);
            currentClient.Value.Returns(client);
            gamesManager.GetGameByName(existingGameName).Returns(game);
            gamesManager.GetGameByName(nonExistingGameName).ReturnsNull();
            game.GameMaster.Returns(gameMaster);

            messageHandler = new JoinGameMessageHandler(currentClient, gamesManager);
        }

        [TestMethod]
        public void Send_RejectJoiningGame_message_when_invalid_name()
        {
            var message = GetMessage(nonExistingGameName);

            messageHandler.Handle(message);

            client.Received().Write(Arg.Is<RejectJoiningGameMessage>(r => 
                r.GameName == nonExistingGameName
            ));
        }

        [TestMethod]
        public void Pass_JoinGame_message_to_GameMaster_with_valid_playerId()
        {
            var message = GetMessage(existingGameName);

            messageHandler.Handle(message);

            gameMaster.Received().Write(Arg.Is<JoinGameMessage>(j => 
                j.PlayerId == playerId && j.GameName == existingGameName
            ));
        }

        private JoinGameMessage GetMessage(string gameName)
        {
            return new JoinGameMessage()
            {
                GameName = gameName
            };
        }
    }
}
