using System.Runtime.CompilerServices;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class GameMessageHandler : MessageHandler<GameMessage>
    {
        private readonly IGamesManager gamesManager;
        private readonly IClient currentClient;

        public GameMessageHandler(IGamesManager gamesManager, ICurrentClient currentClient)
        {
            this.gamesManager = gamesManager;
            this.currentClient = currentClient.Value;
        }

        public override void Handle(GameMessage message)
        {
            if (currentClient.PlayerGuid == message.PlayerGuid)
            {
                var game = gamesManager.GetGameById(message.GameId);

                if (game != null)
                {
                    game.GameMaster.Write(message);
                }
            }
        }
    }
}
