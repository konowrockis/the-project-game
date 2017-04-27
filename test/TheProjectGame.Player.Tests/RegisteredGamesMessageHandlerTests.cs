using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;
using TheProjectGame.Player.MessageHandlers;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.Tests
{
    [TestClass]
    public class RegisteredGamesMessageHandlerTests
    {
        private const string NameOfTheGame = nameof(NameOfTheGame);
        private const uint RetryJoinGameInterval = 12380;

        private readonly IMessageWriter writer;
        private readonly RegisteredGamesMessageHandler handler;

        public RegisteredGamesMessageHandlerTests()
        {
            writer = Substitute.For<IMessageWriter>();

            var options = new PlayerOptions()
            {
                NameOfTheGame = NameOfTheGame,
                TeamColor = "blue",
                Role = "leader",
                RetryJoinGameInterval = RetryJoinGameInterval
            };

            handler = new RegisteredGamesMessageHandler(writer, options);
        }

        [TestMethod]
        public void Resend_GetGames_when_received_empty_list()
        {
            var message = GetMessage();

            handler.Handle(message);

            writer.Received().Write(Arg.Any<GetGamesMessage>(), Arg.Is<double>(RetryJoinGameInterval));
        }

        [TestMethod]
        public void Send_proper_JoinGame_message_after_receiving_game_list()
        {
            var message = GetMessage(NameOfTheGame);

            handler.Handle(message);

            writer.Received().Write(Arg.Is<JoinGameMessage>(m =>
                m.GameName == NameOfTheGame &&
                m.PreferedRole == PlayerType.Leader &&
                m.PreferedTeam == TeamColor.Blue));
        }

        private RegisteredGamesMessage GetMessage(params string[] gameNames)
        {
            return new RegisteredGamesMessage()
            {
                GameInfo = gameNames.Select(game => new GameInfo() { Name = game }).ToList()
            };
        }
    }
}
