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

        public PlayerEventHandler(MessageStream.Factory messageStreamFactory)
        {
            this.messageStreamFactory = messageStreamFactory;
        }
        
        public void OnOpen(IConnection connection, Stream stream)
        {
            Console.WriteLine("Connected");

            MessageStream messages = messageStreamFactory(stream);

            while (true)
            {
                Console.WriteLine("Message received.");
                var msg = messages.Read();

                Console.WriteLine("Sending GetGames message.");
                messages.Write(new GetGames());
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
