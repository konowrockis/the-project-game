using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class PlayerMessageHandler : MessageHandler<PlayerMessage>
    {
        private readonly IClientsManager clientsManager;

        public PlayerMessageHandler(IClientsManager clientsManager)
        {
            this.clientsManager = clientsManager;
        }

        public override void Handle(PlayerMessage message)
        {
            var client = clientsManager.GetPlayerById(message.PlayerId);
            if (client != null)
            {
                client.Write(message);
            }
        }
    }
}
