using System;
using System.Net;
using System.Threading;
using Autofac;
using Serilog;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network.Internal.Server
{
    internal class ServerHandler : INetworkHandler
    {
        private readonly ILogger logger = Log.ForContext<ServerHandler>();

        private readonly IServerSocket server;
        private readonly int port;
        private readonly ClientHandler.Factory clientHandlerFactory;

        public ServerHandler(IServerSocket server, ClientHandler.Factory clientHandlerFactory, GeneralOptions networkOptions)
        {
            this.port = networkOptions.Port;
            this.server = server;
            this.clientHandlerFactory = clientHandlerFactory;
        }

        public void Run()
        {
            logger.Debug("Server started");
            try
            {
                Work();
            }
            catch (Exception e)
            {
                logger.Fatal("Server fatal exception {@Exception}", e);
            }
            logger.Debug("Server stopped");
        }

        private void Work()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            server.Listen(port);

            while (true)
            {
                IClientSocket client = server.Accept();

                var clientThread = new Thread(() => 
                {
                    clientHandlerFactory(client).Run();
                });

                clientThread.Start();
            }
        }
    }
}
