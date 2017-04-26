using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class ConfirmGameRegistrationMessageHandler : MessageHandler<ConfirmGameRegistrationMessage>
    {
        private readonly GameOptions gameOptions;
        private readonly IGameCreator gameCreator;

        public ConfirmGameRegistrationMessageHandler(IGameCreator gameCreator, GameMasterOptions gameOptions)
        {
            this.gameCreator = gameCreator;
            this.gameOptions = gameOptions.GameDefinition;
        }

        public override void Handle(ConfirmGameRegistrationMessage message)
        {
            var game = new GameState(message.GameId, gameOptions.BoardWidth,
                gameOptions.TaskAreaLength, gameOptions.GoalAreaLength, gameOptions.ShamProbability);

            gameCreator.SetCurrentGame(game);
        }
    }
}
