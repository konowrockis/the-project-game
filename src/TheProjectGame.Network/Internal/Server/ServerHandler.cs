using System;
using System.Net;
using System.Threading;
using Autofac;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network.Internal.Server
{
    internal class ServerHandler : SynchronousServerEventWrapper, INetworkHandler
    {
        private readonly IServerSocket server;
        private readonly int port;
        private readonly ILifetimeScope currentScope;

        public ServerHandler(IServerSocket server, IServerEventHandler eventHandler, 
            ILifetimeScope currentScope, IOptions<NetworkOptions> networkOptions) : base(eventHandler)
        {
            this.port = networkOptions.Value.Port;
            this.server = server;
            this.currentScope = currentScope;
        }

        public void Run()
        {
            this.OnServerStart();
            try
            {
                Work();
            }
            catch (System.Exception e)
            {
                this.OnServerError(e);
            }
            this.OnServerStop();
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
                    using (var scope = currentScope.BeginLifetimeScope())
                    {
                        var clientHandlerFactory = scope.Resolve<ClientHandler.Factory>();
                        var clientHandler = clientHandlerFactory(client, this);

                        clientHandler.Run();
                    }
                });

                clientThread.Start();
            }
        }
    }
}
