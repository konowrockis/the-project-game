using System;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class RejectGameRegistrationMessageHandler : MessageHandler<RejectGameRegistration>
    {
        private readonly IMessageWriter messageWriter;

        public RejectGameRegistrationMessageHandler(IMessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public override void Handle(RejectGameRegistration message)
        {
            var response = new RegisterGame()
            {
                NewGameInfo = new GameInfo()
                {
                    Name = Guid.NewGuid().ToString(),
                    BlueTeamPlayers = 10,
                    RedTeamPlayers = 10
                }
            };

            messageWriter.Write(response, 100);
        }
    }
}
