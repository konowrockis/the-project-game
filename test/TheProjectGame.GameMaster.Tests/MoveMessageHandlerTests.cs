using System;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Client;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
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
        private const uint MoveDelay = 12356;

        private readonly IMessageWriter writer;
        private readonly IGameState gameState;
        private readonly IPlayersMap playersMap;
        private readonly MoveMessageHandler handler;

        public MoveMessageHandlerTests()
        {
            writer = Substitute.For<IMessageWriter>();
            gameState = new GameState(GameId, 10, 10, 10, 0);
            playersMap = new PlayersMap();

            ICurrentGame currentGame = Substitute.For<ICurrentGame>();
            var options = new GameMasterOptions()
            {
                ActionCosts = new ActionCostsOptions()
                {
                    MoveDelay = MoveDelay
                }
            };

            currentGame.Game.Returns(gameState);
            currentGame.Players.Returns(playersMap);

            handler = new MoveMessageHandler(writer, options, currentGame, () => GetDataBuilder());
        }

        [TestMethod]
        public void Send_proper_response_to_valid_move_message()
        {
            var board = gameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);
            string guid = playersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Down, GameId, guid);

            handler.Handle(message);

            Assert.IsNull(gameState.Board.Fields[0, 0].Player);
            Assert.AreEqual(player, gameState.Board.Fields[0, 1].Player);
            writer.Received().Write(Arg.Is<DataMessage>(data =>
                data.GameFinished == false &&
                (data.Pieces == null || data.Pieces.Count == 0) &&
                data.PlayerId == player.Id &&
                data.PlayerLocation.X == 0 &&
                data.PlayerLocation.Y == 1 &&
                (data.TaskFields == null || data.TaskFields.Count == 0) &&
                data.GoalFields != null &&
                data.GoalFields.Count == 1 &&
                data.GoalFields.First().PlayerId == player.Id &&
                data.GoalFields.First().X == 0 &&
                data.GoalFields.First().Y == 1),
                Arg.Is<double>(delay => delay.Equals(MoveDelay))
            );
        }

        [TestMethod]
        public void Send_proper_response_to_invalid_move_message()
        {
            var board = gameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);
            string guid = playersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Up, GameId, guid);

            handler.Handle(message);

            Assert.AreEqual(player, gameState.Board.Fields[0, 0].Player);
            writer.Received().Write(Arg.Is<DataMessage>(data =>
                data.GameFinished == false &&
                (data.Pieces == null || data.Pieces.Count == 0) &&
                data.PlayerId == player.Id &&
                data.PlayerLocation.X == 0 &&
                data.PlayerLocation.Y == 0 &&
                (data.TaskFields == null || data.TaskFields.Count == 0) &&
                (data.GoalFields == null || data.GoalFields.Count == 0)), 
                Arg.Is<double>(delay => delay.Equals(MoveDelay))
            );
        }

        [TestMethod]
        public void Send_proper_response_to_move_message_when_position_occupied_no_pieces()
        {
            var board = gameState.Board;
            GamePlayer player = CreatePlayer(1, 0, 0, board);
            GamePlayer enemy = CreatePlayer(2, 0, 1, board);
            string guid = playersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Down, GameId, guid);

            handler.Handle(message);

            Assert.AreEqual(player, gameState.Board.Fields[0, 0].Player);
            Assert.AreEqual(enemy, gameState.Board.Fields[0, 1].Player);
            writer.Received().Write(Arg.Is<DataMessage>(data =>
                data.GameFinished == false &&
                (data.Pieces == null || data.Pieces.Count == 0) &&
                data.PlayerId == player.Id &&
                data.PlayerLocation.X == 0 &&
                data.PlayerLocation.Y == 0 &&
                (data.TaskFields == null || data.TaskFields.Count == 0) &&
                data.GoalFields != null &&
                data.GoalFields.Count == 1 &&
                data.GoalFields.First().PlayerId == enemy.Id &&
                data.GoalFields.First().X == 0 &&
                data.GoalFields.First().Y == 1),
                Arg.Is<double>(delay => delay.Equals(MoveDelay))
            );
        }

        private DataBuilder GetDataBuilder()
        {
            return new DataBuilder(
                new MapperConfiguration(cfg =>
                    cfg.AddProfile(new ClientProfile(true)))
                .CreateMapper()
            );
        }

        private MoveMessage CreateMessage(MoveType direction, ulong gameId, string playerGuid)
        {
            return new MoveMessage()
            {
                Direction = direction,
                GameId = gameId,
                PlayerGuid = playerGuid
            };
        }

        private GamePlayer CreatePlayer(ulong id, uint x, uint y, IBoard board)
        {
            var player = new GamePlayer(id)
            {
                Position = new Position(x,y)
            };
            board.MovePlayer(player, player.Position);
            return player;
        }
    }
}
