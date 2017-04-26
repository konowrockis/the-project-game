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
    public class RegisterGameMessageHandlerTests
    {
        const string existingGameName = nameof(existingGameName);
        const string nonExistentGameName = nameof(nonExistentGameName);

        const ulong blueTeamPlayers = 5;
        const ulong redTeamPlayers = 6;
        const uint nextGameId = 8;

        private IClient client;
        private IGamesManager gamesManager;
        private RegisterGameMessageHandler messageHandler;

        public RegisterGameMessageHandlerTests()
        {
            client = Substitute.For<IClient>();
            gamesManager = Substitute.For<IGamesManager>();

            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            IGame game = Substitute.For<IGame>();

            gamesManager.GetGameByName(existingGameName).Returns(game);
            gamesManager.GetGameByName(nonExistentGameName).ReturnsNull();
            gamesManager.GetNewGameId().Returns(nextGameId);
            currentClient.Value.Returns(client);

            messageHandler = new RegisterGameMessageHandler(currentClient, gamesManager);
        }

        [TestMethod]
        public void Reject_game_registration_if_game_with_provided_name_exists()
        {
            var message = GetMessage(existingGameName);

            messageHandler.Handle(message);

            client.Received().Write(Arg.Is<RejectGameRegistrationMessage>(m => m.GameName == existingGameName));
        }

        [TestMethod]
        public void Send_confirm_game_registration_if_game_name_is_valid()
        {
            var message = GetMessage(nonExistentGameName);

            messageHandler.Handle(message);

            client.Received().Write(Arg.Is<ConfirmGameRegistrationMessage>(m => m.GameId == nextGameId));
        }

        [TestMethod]
        public void Add_game_to_registry_if_game_name_is_valid()
        {
            var message = GetMessage(nonExistentGameName);

            messageHandler.Handle(message);

            gamesManager.Received().Add(Arg.Is<IGame>(g =>
                g.Id == nextGameId && 
                g.Name == nonExistentGameName && 
                g.GameMaster == client &&
                g.RedTeamPlayers == redTeamPlayers && 
                g.BlueTeamPlayers == blueTeamPlayers
            ));
        }

        private RegisterGameMessage GetMessage(string gameName)
        {
            return new RegisterGameMessage()
            {
                NewGameInfo =  new GameInfo()
                {
                    Name = gameName,
                    BlueTeamPlayers = blueTeamPlayers,
                    RedTeamPlayers = redTeamPlayers
                }
            };
        }
    }
}
