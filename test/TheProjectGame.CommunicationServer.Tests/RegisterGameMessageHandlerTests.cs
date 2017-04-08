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

        [TestMethod]
        public void Reject_game_registration_if_game_with_provided_name_exists()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(existingGameName);

            sut.MessageHandler.Handle(message);

            sut.Client.Received().Write(Arg.Is<RejectGameRegistration>(m => m.GameName == existingGameName));
        }

        [TestMethod]
        public void Send_confirm_game_registration_if_game_name_is_valid()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(nonExistentGameName);

            sut.MessageHandler.Handle(message);

            sut.Client.Received().Write(Arg.Is<ConfirmGameRegistration>(m => m.GameId == nextGameId));
        }

        [TestMethod]
        public void Add_game_to_registry_if_game_name_is_valid()
        {
            var sut = GetMessageHandler();
            var message = GetMessage(nonExistentGameName);

            sut.MessageHandler.Handle(message);

            sut.GamesManager.Received().Add(Arg.Is<IGame>(g =>
                g.Id == nextGameId && g.Name == nonExistentGameName && g.GameMaster == sut.Client &&
                g.RedTeamPlayers == redTeamPlayers && g.BlueTeamPlayers == blueTeamPlayers));
        }

        private SystemUnderTests GetMessageHandler()
        {
            IClient client = Substitute.For<IClient>();
            ICurrentClient currentClient = Substitute.For<ICurrentClient>();
            IGamesManager gamesManager = Substitute.For<IGamesManager>();
            IGame game = Substitute.For<IGame>();

            gamesManager.GetGameByName(existingGameName).Returns(game);
            gamesManager.GetGameByName(nonExistentGameName).ReturnsNull();
            gamesManager.GetNewGameId().Returns(nextGameId);
            currentClient.Value.Returns(client);

            return new SystemUnderTests()
            {
                MessageHandler = new RegisterGameMessageHandler(currentClient, gamesManager),
                Client = client,
                GamesManager = gamesManager
            };
        }

        private RegisterGame GetMessage(string gameName)
        {
            return new RegisterGame()
            {
                NewGameInfo =  new GameInfo()
                {
                    Name = gameName,
                    BlueTeamPlayers = blueTeamPlayers,
                    RedTeamPlayers = redTeamPlayers
                }
            };
        }

        private class SystemUnderTests
        {
            public RegisterGameMessageHandler MessageHandler { get; set; }
            public IClient Client { get; set; }
            public IGamesManager GamesManager { get; set; }
        }
    }
}
