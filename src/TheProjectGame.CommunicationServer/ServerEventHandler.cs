using System;
using System.IO;
using Autofac;
using Serilog;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.CommunicationServer
{
    class ServerEventHandler : IClientEventHandler
    {
        private readonly ILogger logger = Log.ForContext<ServerEventHandler>();

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
            logger.Debug("New connection {@Address}:{@Port}", connection.Address(), connection.Port());

            var messageStream = messageStreamFactory(stream);
            var client = serverClientFactory(messageStream);

            clientsManager.Add(client);

            scope.InjectProperties(currentClient).Value = client;
            
            client.Start();
        }

        public void OnClose(IConnectionData data)
        {
            logger.Debug("Connection closed {@Address}:{@Port}", data.Address(), data.Port());

            clientsManager.Remove(currentClient.Value);
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            logger.Error("Client {@Address}:{@Port} caused exception {@Exception}", data.Address(), data.Port(), exception);

            clientsManager.Remove(currentClient.Value);
        }
    }
}