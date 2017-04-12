using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
    public class PlacePieceMessageHandlerTests
    {
        private const uint PlacingDelay = 100;
        private const ulong GameId = 1;
        private const ulong PlayerId = 1;

        private readonly string PlayerGuid = Guid.NewGuid().ToString();

        [TestMethod]
        public void Player_not_holding_any_piece()
        {
            var systemUnderTests = Setup();
            var board = systemUnderTests.Board;
            var writer = systemUnderTests.Writer;
            board.Pieces.Returns(new List<BoardPiece>());

            systemUnderTests.Handler.Handle(CreateMessage());

            writer.Received()
                .Write(
                    Arg.Is<Data>(
                        data =>
                            !data.GameFinished && data.PlayerId == PlayerId && data.GoalFields == null &&
                            data.PlayerLocation == null && data.TaskFields == null && data.Pieces == null),
                    Arg.Is<double>(PlacingDelay));
        }

        private SystemUnderTests Setup()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            ActionCostsOptions options = new ActionCostsOptions();
            IPlayersMap playersMap = Substitute.For<IPlayersMap>();
            IGameState gameState = Substitute.For<IGameState>();
            GamePlayer player = new GamePlayer(PlayerId);
            IBoard board = Substitute.For<IBoard>();
            ICurrentGame currentGame = Substitute.For<ICurrentGame>();

            currentGame.Game.Returns(gameState);
            currentGame.Players.Returns(playersMap);
            playersMap.GetPlayer(PlayerGuid).Returns(player);
            gameState.Id.Returns(GameId);
            gameState.Board.Returns(board);
            gameState.Players.Returns(new List<GamePlayer>() {player});

            PlacePieceMessageHandler handler = new PlacePieceMessageHandler(writer, options, currentGame);

            return new SystemUnderTests(writer, board, player, handler);
        }

        private PlacePiece CreateMessage()
        {
            return new PlacePiece()
            {
                GameId = GameId,
                PlayerGuid = PlayerGuid
            };
        }

        private class SystemUnderTests
        {
            public IMessageWriter Writer { get; private set; }
            public IBoard Board { get; private set; }
            public GamePlayer Player { get; private set; }
            public PlacePieceMessageHandler Handler { get; private set; }

            public SystemUnderTests(IMessageWriter writer, IBoard board, GamePlayer player,
                PlacePieceMessageHandler handler)
            {
                Writer = writer;
                Board = board;
                Player = player;
                Handler = handler;
            }
        }
    }
}