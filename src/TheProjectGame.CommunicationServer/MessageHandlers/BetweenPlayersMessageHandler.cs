using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class BetweenPlayersMessageHandler : MessageHandler<BetweenPlayersMessage>
    {
        private readonly IClientsManager clientsManager;

        public BetweenPlayersMessageHandler(
            IClientsManager clientsManager)
        {
            this.clientsManager = clientsManager;
        }

        public override void Handle(BetweenPlayersMessage message)
        {
            var client = clientsManager.GetPlayerById(message.PlayerId);
            if (client != null)
            {
                client.Write(message);
            }
        }
    }
}
