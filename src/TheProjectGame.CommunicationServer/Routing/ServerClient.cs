using System;
using TheProjectGame.Contracts;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.Routing
{
    class ServerClient : IClient
    {
        private readonly MessageStream messageStream;
        private readonly IMessageExecutor messageExecutor;

        public ulong? GameId { get; private set; }
        public string PlayerGuid { get; private set; }
        public ulong PlayerId { get; private set; }

        public delegate ServerClient Factory(MessageStream messageStream);

        public ServerClient(MessageStream messageStream, IMessageExecutor messageExecutor, IClientsManager clientsManager)
        {
            this.messageStream = messageStream;
            this.messageExecutor = messageExecutor;
            PlayerId = clientsManager.GetNewPlayerId();
        }

        public void DisconnectFromGame()
        {
            GameId = null;
            PlayerGuid = null;
        }

        public void JoinGame(ulong gameId)
        {
            GameId = gameId;
            PlayerGuid = Guid.NewGuid().ToString();
        }

        public void Start()
        {
            while (true)
            {
                var message = messageStream.Read();

                messageExecutor.Execute(message);
            }
        }

        public void Write(IMessage message)
        {
            messageStream.Write(message);
        }
    }
}
