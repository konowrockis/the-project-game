using System;
using System.Net;
using System.Threading;
using Autofac;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network.Internal.Server
{
    internal class ServerHandler : INetworkHandler
    {
        private readonly IServerSocket server;
        private readonly int port;
        private readonly ClientHandler.Factory clientHandlerFactory;

        public ServerHandler(IServerSocket server, ClientHandler.Factory clientHandlerFactory, IOptions<NetworkOptions> networkOptions)
        {
            this.port = networkOptions.Value.Port;
            this.server = server;
            this.clientHandlerFactory = clientHandlerFactory;
        }

        public void Run()
        {
            Console.WriteLine("Server started");
            try
            {
                Work();
            }
            catch (Exception e)
            {
                Console.WriteLine("Server error: {0}", e.Message);
            }
            Console.WriteLine("Server stopped");
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
