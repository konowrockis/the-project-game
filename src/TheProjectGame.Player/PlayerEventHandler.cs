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
        private readonly MessageStream.Factory messageStreamFactory;
        private readonly IMessageExecutor messageExecutor;

        public PlayerEventHandler(MessageStream.Factory messageStreamFactory, IMessageExecutor messageExecutor)
        {
            this.messageStreamFactory = messageStreamFactory;
            this.messageExecutor = messageExecutor;
        }
        
        public void OnOpen(IConnection connection, Stream stream)
        {
            Console.WriteLine("Connected");

            MessageStream messages = messageStreamFactory(stream);

            while (true)
            {
                var message = messages.Read();

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
