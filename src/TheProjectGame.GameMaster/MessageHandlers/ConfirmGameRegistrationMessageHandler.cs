using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class ConfirmGameRegistrationMessageHandler : MessageHandler<ConfirmGameRegistration>
    {
        private readonly IMessageWriter messageWriter;
        private readonly GameOptions gameOptions;
        private readonly IGameCreator gameCreator;

        public ConfirmGameRegistrationMessageHandler(IMessageWriter messageWriter, IGameCreator gameCreator, GameMasterOptions gameOptions)
        {
            this.messageWriter = messageWriter;
            this.gameCreator = gameCreator;
            this.gameOptions = gameOptions.GameDefinition;
        }

        public override void Handle(ConfirmGameRegistration message)
        {
            GameState game = new GameState(message.GameId, gameOptions.BoardWidth, 
                gameOptions.TaskAreaLength, gameOptions.GoalAreaLength,gameOptions.ShamProbability);

            gameCreator.SetCurrentGame(game);
        }
    }
}
