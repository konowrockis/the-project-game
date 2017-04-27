using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Client;
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
    public class PlacePieceMessageHandlerTests
    {
        private const uint PlacingDelay = 5987;
        private const ulong GameId = 1;
        private const ulong PlayerId = 1;

        private readonly string PlayerGuid = Guid.NewGuid().ToString();

        private readonly IMessageWriter writer;
        private readonly IBoard board;
        private readonly GamePlayer player;
        private readonly PlacePieceMessageHandler handler;

        public PlacePieceMessageHandlerTests()
        {
            writer = Substitute.For<IMessageWriter>();
            player = new GamePlayer(PlayerId);
            board = Substitute.For<IBoard>();

            ICurrentGame currentGame = Substitute.For<ICurrentGame>();
            IPlayersMap playersMap = Substitute.For<IPlayersMap>();
            IGameState gameState = Substitute.For<IGameState>();
            GameMasterOptions options = new GameMasterOptions()
            {
                ActionCosts = new ActionCostsOptions()
                {
                    PlacingDelay = PlacingDelay
                }
            };

            currentGame.Game.Returns(gameState);
            currentGame.Players.Returns(playersMap);
            playersMap.GetPlayer(PlayerGuid).Returns(player);
            gameState.Id.Returns(GameId);
            gameState.Board.Returns(board);
            gameState.Players.Returns(new List<GamePlayer>() { player });

            handler = new PlacePieceMessageHandler(writer, options, currentGame, () => GetDataBuilder());
        }

        [TestMethod]
        public void Player_not_holding_any_piece()
        {
            board.Pieces.Returns(new List<BoardPiece>());

            handler.Handle(CreateMessage());

            writer.Received()
                .Write(
                    Arg.Is<DataMessage>(
                        data =>
                            !data.GameFinished && 
                            data.PlayerId == PlayerId && 
                            data.GoalFields == null &&
                            data.PlayerLocation == null && 
                            data.TaskFields == null && 
                            data.Pieces == null),
                    Arg.Is<double>(PlacingDelay));
        }

        private DataBuilder GetDataBuilder()
        {
            return new DataBuilder(
                new MapperConfiguration(cfg => 
                    cfg.AddProfile(new ClientProfile(true)))
                .CreateMapper()
            );
        }
        
        private PlacePieceMessage CreateMessage()
        {
            return new PlacePieceMessage()
            {
                GameId = GameId,
                PlayerGuid = PlayerGuid
            };
        }
    }
}