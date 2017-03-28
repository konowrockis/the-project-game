using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Player.MessageHandlers;

namespace TheProjectGame.Player.Tests
{
    [TestClass]
    public class RejectJoiningGameMessageHandlerTests
    {
        [TestMethod]
        public void Write_GetGames_message_after_receiving_RejectJoiningGame()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            GetGames response = null;
            writer.When(w => w.Write(Arg.Any<GetGames>())).Do(c => response = c.Arg<GetGames>());

            new RejectJoiningGameMessageHandler(writer).Handle(new RejectJoiningGame());

            Assert.IsNotNull(response);
        }
        
    }

}
