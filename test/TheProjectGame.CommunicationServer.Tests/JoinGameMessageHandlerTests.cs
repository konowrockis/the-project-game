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

        [TestMethod]
        public void Send_RejectJoiningGame_message_when_invalid_name()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(nonExistingGameName);

            sut.MessageHandler.Handle(message);

            sut.Client.Received().Write(Arg.Is<RejectJoiningGame>(r => 
                r.GameName == nonExistingGameName
            ));
        }

        [TestMethod]
        public void Pass_JoinGame_message_to_GameMaster_with_valid_playerId()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(existingGameName);

            sut.MessageHandler.Handle(message);

            sut.GameMaster.Received().Write(Arg.Is<JoinGame>(j => 
                j.PlayerId == playerId && j.GameName == existingGameName
            ));
        }

        private SystemUnderTests GetMessageHandler()
        {
            var client = Substitute.For<IClient>();
            var currentClient = Substitute.For<ICurrentClient>();
            var gamesManager = Substitute.For<IGamesManager>();
            var game = Substitute.For<IGame>();
            var gameMaster = Substitute.For<IClient>();

            client.PlayerId.Returns(playerId);
            currentClient.Value.Returns(client);
            gamesManager.GetGameByName(existingGameName).Returns(game);
            gamesManager.GetGameByName(nonExistingGameName).ReturnsNull();
            game.GameMaster.Returns(gameMaster);
            
            return new SystemUnderTests()
            {
                MessageHandler = new JoinGameMessageHandler(currentClient, gamesManager),
                GameMaster = gameMaster,
                Client = client
            };
        }

        private JoinGame GetMessage(string gameName)
        {
            return new JoinGame()
            {
                GameName = gameName
            };
        }

        private class SystemUnderTests
        {
            public JoinGameMessageHandler MessageHandler { get; set; }
            public IClient GameMaster { get; set; }
            public IClient Client { get; set; }
        }
    }
}
