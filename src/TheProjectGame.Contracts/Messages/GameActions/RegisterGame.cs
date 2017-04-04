using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class RegisterGame : IMessage
    {
        public GameInfo NewGameInfo { get; set; }
    }
}
