using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.CommunicationServer
{
    class ServerEventHandler : IClientEventHandler
    {
        private readonly IClientsManager clientsManager;
        private readonly ICurrentClient currentClient;
        private readonly ILifetimeScope scope;

        private readonly ServerClient.Factory serverClientFactory;
        private readonly MessageStream.Factory messageStreamFactory;

        public ServerEventHandler(IClientsManager clientsManager, ICurrentClient currentClient, 
            ServerClient.Factory serverClientFactory, MessageStream.Factory messageStreamFactory,
            ILifetimeScope scope)
        {
            this.clientsManager = clientsManager;
            this.currentClient = currentClient;
            this.serverClientFactory = serverClientFactory;
            this.messageStreamFactory = messageStreamFactory;
            this.scope = scope;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            Console.WriteLine("New connection @{0}:{1}", connection.Address(), connection.Port());

            var messageStream = messageStreamFactory(stream);
            var client = serverClientFactory(messageStream);

            clientsManager.Add(client);

            scope.InjectProperties(currentClient).Value = client;
            
            client.Start();
        }

        public void OnClose(IConnectionData data)
        {
            Console.WriteLine("Connection closed @{0}:{1}", data.Address(), data.Port());

            clientsManager.Remove(currentClient.Value);
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            Console.WriteLine("Client @{0}:{1} error {2}", data.Address(), data.Port(), exception);

            clientsManager.Remove(currentClient.Value);
        }
    }
}