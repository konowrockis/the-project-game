using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.Player
{
    class PlayerEventHandler : IClientEventHandler
    {
        private readonly IMessageReader messageReader;
        private readonly IMessageExecutor messageExecutor;
        private readonly IMessageProxyCreator proxyCreator;

        public PlayerEventHandler(IMessageReader messageReader, IMessageProxyCreator proxyCreator, IMessageExecutor messageExecutor)
        {
            this.messageReader = messageReader;
            this.proxyCreator = proxyCreator;
            this.messageExecutor = messageExecutor;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            Console.WriteLine("Connected");

            proxyCreator.SetStream(stream);

            while (true)
            {
                var message = messageReader.Read();

                messageExecutor.Execute(message);
            }
        }

        public void OnClose(IConnectionData data)
        {
            Console.WriteLine("Disconnected");
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            Console.WriteLine("Error = {0}", exception);
        }
    }
}
