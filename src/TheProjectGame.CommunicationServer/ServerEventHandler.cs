using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.CommunicationServer
{
    class ServerEventHandler : IServerEventHandler
    {
        private readonly MessageStream.Factory messageStreamFactory;

        public ServerEventHandler(MessageStream.Factory messageStreamFactory)
        {
            this.messageStreamFactory = messageStreamFactory;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            Console.WriteLine("New connection @{0}:{1}", connection.Address(), connection.Port());

            MessageStream messages = messageStreamFactory(stream);

            while (true)
            {
                Console.WriteLine("Sending GetGames message.");
                messages.Write(new GetGames(), 1000);

                var msg = messages.Read();
                Console.WriteLine("Message received.");
            }
        }

        public void OnClose(IConnectionData data)
        {
            Console.WriteLine("Connection closed @{0}:{1}", data.Address(), data.Port());
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            Console.WriteLine("Client @{0}:{1} error {2}", data.Address(), data.Port(), exception);
        }

        public void OnServerStart()
        {
            Console.WriteLine("Server started");
        }

        public void OnServerError(Exception exception)
        {
            Console.WriteLine("Server error: {0}", exception.Message);
        }

        public void OnServerStop()
        {
            Console.WriteLine("Server stopped");
        }
    }
}