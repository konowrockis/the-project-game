using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.Player.MessageHandlers
{
    class RejectJoiningGameMessageHandler : MessageHandler<RejectJoiningGame>
    {
        private readonly IMessageWriter messageWriter;

        public RejectJoiningGameMessageHandler(IMessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public override void Handle(RejectJoiningGame message)
        {
            messageWriter.Write(new GetGames());
        }
    }
}
