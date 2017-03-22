using System;
using System.Threading.Tasks;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.Routing
{
    class ServerClient : IClient
    {
        private readonly MessageStream messageStream;
        private readonly IMessageExecutor messageExecutor;

        public ulong? GameId { get; private set; }
        public string PlayerGuid { get; private set; }
        public ulong? PlayerId { get; private set; }

        public delegate ServerClient Factory(MessageStream messageStream);

        public ServerClient(MessageStream messageStream, IMessageExecutor messageExecutor)
        {
            this.messageStream = messageStream;
            this.messageExecutor = messageExecutor;
        }

        public void DisconnectFromGame()
        {
            GameId = null;
            PlayerGuid = null;
            PlayerId = null;
        }

        public void JoinGame(ulong gameId)
        {
            GameId = gameId;
            PlayerId = 0; // TODO: get from game manager
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
            Task.Delay(TimeSpan.FromMilliseconds(100)).ContinueWith((t) =>
            {
                try
                {
                    messageStream.Write(message);
                }
                catch { }
            });
        }
    }
}
