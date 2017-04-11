using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.MessageHandlers;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.Tests
{
    [TestClass]
    public class MoveMessageHandlerTests
    {
        private const ulong GameId = 1;
        private const double MoveDelay = 100;

        [TestMethod]
        public void Send_proper_response_to_valid_move_message()
        {
            var systemUnderTests = TestSetup();
            var board = systemUnderTests.GameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);
            string guid = systemUnderTests.PlayersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Down, GameId, guid);
            Predicate<Data> assertValidResponse = data =>
                data.GameFinished == false
                && (data.Pieces == null || data.Pieces.Count == 0)
                && data.PlayerId == player.Id
                && data.PlayerLocation.X == 0
                && data.PlayerLocation.Y == 1
                && (data.TaskFields == null || data.TaskFields.Count == 0)
                && data.GoalFields != null
                && data.GoalFields.Count == 1
                && data.GoalFields.First().PlayerId == player.Id
                && data.GoalFields.First().X == 0
                && data.GoalFields.First().Y == 1;

            systemUnderTests.Handler.Handle(message);

            Assert.IsTrue(systemUnderTests.GameState.Board.Fields[0,0].Player==null);
            Assert.IsTrue(systemUnderTests.GameState.Board.Fields[0, 1].Player == player);
            systemUnderTests.Writer.Received().Write(Arg.Is<Data>(data=>assertValidResponse(data)),Arg.Is<double>(delay=>delay.Equals(MoveDelay)));
        }

        [TestMethod]
        public void Send_proper_response_to_invalid_move_message()
        {
            var systemUnderTests = TestSetup();
            var board = systemUnderTests.GameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);

            string guid = systemUnderTests.PlayersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Up, GameId, guid);

            systemUnderTests.Handler.Handle(message);

            Predicate<Data> assertValidResponse = data =>
                data.GameFinished == false
                && (data.Pieces == null || data.Pieces.Count == 0)
                && data.PlayerId == player.Id
                && data.PlayerLocation.X == 0
                && data.PlayerLocation.Y == 0
                && (data.TaskFields == null || data.TaskFields.Count == 0)
                && (data.GoalFields == null || data.GoalFields.Count == 0);

            Assert.IsTrue(systemUnderTests.GameState.Board.Fields[0, 0].Player == player);
            systemUnderTests.Writer.Received().Write(Arg.Is<Data>(data => assertValidResponse(data)), Arg.Is<double>(delay => delay.Equals(MoveDelay)));
        }

        [TestMethod]
        public void Send_proper_response_to_move_message_when_position_occupied_no_pieces()
        {
            var systemUnderTests = TestSetup();
            var board = systemUnderTests.GameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);
            GamePlayer enemy = CreatePlayer(2, 0, 1, board);

            string guid = systemUnderTests.PlayersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Down, GameId, guid);

            systemUnderTests.Handler.Handle(message);

            Predicate<Data> assertValidResponse = data =>
                data.GameFinished == false
                && (data.Pieces == null || data.Pieces.Count == 0)
                && data.PlayerId == player.Id
                && data.PlayerLocation.X == 0
                && data.PlayerLocation.Y == 0
                && (data.TaskFields == null || data.TaskFields.Count == 0)
                && data.GoalFields != null
                && data.GoalFields.Count == 1
                && data.GoalFields.First().PlayerId == enemy.Id
                && data.GoalFields.First().X == 0
                && data.GoalFields.First().Y == 1;

            Assert.IsTrue(systemUnderTests.GameState.Board.Fields[0, 0].Player == player);
            Assert.IsTrue(systemUnderTests.GameState.Board.Fields[0, 1].Player == enemy);
            systemUnderTests.Writer.Received().Write(Arg.Is<Data>(data => assertValidResponse(data)), Arg.Is<double>(delay => delay.Equals(MoveDelay)));
        }

        private Move CreateMessage(MoveType direction, ulong gameId, string playerGuid)
        {
            return new Move()
            {
                Direction = direction,
                GameId = gameId,
                PlayerGuid = playerGuid
            };
        }

        private SystemUnderTests TestSetup()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            GameState state = new GameState(GameId, 10, 10, 10);
            var playersMap = new PlayersMap();
            MoveMessageHandler handler = new MoveMessageHandler(writer, new ActionCostsOptions(),state,playersMap);
            return new SystemUnderTests(handler,writer,state,playersMap);
        }

        private GamePlayer CreatePlayer(ulong id, uint x, uint y, Board board)
        {
            var player = new GamePlayer(id)
            {
                Position = new Position(x,y)
            };
            board.MovePlayer(player, player.Position);
            return player;
        }

        private class SystemUnderTests
        {
            public MoveMessageHandler Handler { get;  }
            public IMessageWriter Writer { get;  }
            public GameState GameState { get; }
            public PlayersMap PlayersMap { get; }

            public SystemUnderTests(MoveMessageHandler handler, IMessageWriter writer, GameState gameState, PlayersMap playersMap)
            {
                Handler = handler;
                Writer = writer;
                GameState = gameState;
                PlayersMap = playersMap;
            }
        }



    }
}
