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
        [TestMethod]
        public void NoGamesTest()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            GetGames response = null;
            writer.When(w => w.Write(Arg.Any<GetGames>(),Arg.Any<double>())).Do(c => response = c.Arg<GetGames>());
            new RegisteredGamesMessageHandler(writer,null).Handle(new RegisteredGames() {GameInfo = new List<GameInfo>()});
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SendJoinGameMessageTest()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();

            PlayerOptions options = new PlayerOptions();
            options.TeamColor = "blue";
            options.Role = "leader";
            GameInfo info = new GameInfo();
            info.Name = "test";

            JoinGame response = null;
            writer.When(w => w.Write(Arg.Any<JoinGame>())).Do(c => response = c.Arg<JoinGame>());
            
            new RegisteredGamesMessageHandler(writer, options).Handle(new RegisteredGames() { GameInfo = new List<GameInfo>() {info} });
            Assert.IsNotNull(response);
            Assert.IsTrue(response.GameName=="test");
            Assert.IsTrue(response.PreferedRole==PlayerType.Leader);
            Assert.IsTrue(response.PreferedTeam == TeamColor.Blue);
        }
    }
}
