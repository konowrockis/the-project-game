using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class JoinGameMessageHandler : MessageHandler<JoinGameMessage>
    {
        private readonly IClient currentClient;
        private readonly IGamesManager gamesManager;

        public JoinGameMessageHandler(ICurrentClient currentClient, IGamesManager gamesManager)
        {
            this.currentClient = currentClient.Value;
            this.gamesManager = gamesManager;
        }

        public override void Handle(JoinGameMessage message)
        {
            var game = gamesManager.GetGameByName(message.GameName);
            if (game == null)
            {
                var response = new RejectJoiningGameMessage()
                {
                    GameName = message.GameName,
                    PlayerId = 0
                };

                currentClient.Write(response);
            }
            else
            {
                message.PlayerId = currentClient.PlayerId;
                message.PlayerIdSpecified = true;
                game.GameMaster.Write(message);
            }
        }
    }
}
