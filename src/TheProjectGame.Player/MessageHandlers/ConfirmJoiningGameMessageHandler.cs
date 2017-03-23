using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class ConfirmJoiningGameMessageHandler : MessageHandler<ConfirmJoiningGame>
    {
        private readonly IMessageWriter messageWriter;
        private readonly PlayerOptions playerOptions;

        public ConfirmJoiningGameMessageHandler(IMessageWriter messageWriter, IOptions<PlayerOptions> playerOptions)
        {
            this.messageWriter = messageWriter;
            this.playerOptions = playerOptions.Value;
        }

        public override void Handle(ConfirmJoiningGame message)
        {

        }
    }
}
