using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;
using TheProjectGame.Player.MessageHandlers;
using TheProjectGame.Game;
using System.Collections.Generic;

namespace TheProjectGame.Player.Tests
{
    [TestClass]
    public class AcceptKnowledgeExchangeMessageHandlerTests
    {
        [TestMethod]
        public void Send_DataMessage_after_receiving_AcceptExchangeRequestMessage()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            var gameState = new GameState(1);
            var knowledge = new PlayerKnowledge();
            var player = new GamePlayer(1);
            var board = new Board(10, 2, 2, .5);
            var handler = new AcceptKnowlegdeExchangeMessageHandler(writer, knowledge);
            var message = new AcceptExchangeRequestMessage();
            var playersList = new List<GamePlayer>(1);

           
            playersList.Add(player);
            board.Init(playersList, 5, 5);
            board.PlaceNewPiece();
            gameState.Board = board;
            knowledge.Init(player, "test", gameState);
            handler.Handle(message);

            writer.Received().Write(Arg.Any<DataMessage>());
        }
    }
}
