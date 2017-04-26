using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.MessageHandlers;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.Tests
{
    [TestClass]
    public class ConfirmGameRegistrationMessageHandlerTests
    {
        private const int gameId = 1;
        private const int RetryRegisterGameInterval = 5000;
        private const uint BoardWidth = 10;
        private const uint GoalAreaLength = 3;
        private const uint TaskAreaLength = 15;
        private const double ShamProbability = 0.3;
        
        [TestMethod]
        public void Set_current_game_when_confirmation_is_received()
        {
            IGameCreator gameCreator = Substitute.For<IGameCreator>();
            var handler = new ConfirmGameRegistrationMessageHandler(gameCreator, GetOptions());
            var message = new ConfirmGameRegistrationMessage() { GameId = gameId };

            handler.Handle(message);

            gameCreator.Received().SetCurrentGame(Arg.Is<IGameState>(game => 
                game.Id == gameId &&
                game.Board.BoardWidth == BoardWidth &&
                game.Board.GoalAreaHeight == GoalAreaLength &&
                game.Board.TaskAreaHeight == TaskAreaLength
            ));
        }

        private GameMasterOptions GetOptions()
        {
            return new GameMasterOptions()
            {
                RetryRegisterGameInterval = RetryRegisterGameInterval,
                GameDefinition = new GameOptions()
                {
                    BoardWidth = BoardWidth,
                    TaskAreaLength = TaskAreaLength,
                    GoalAreaLength = GoalAreaLength,
                    ShamProbability = ShamProbability
                }
            };
        }
    }
}
