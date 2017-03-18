using System;
using System.Net;
using System.Threading;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network.Internal.Server
{
    internal class ServerHandler : SynchronousServerEventWrapper, INetworkHandler
    {
        private readonly IServerSocket server;
        private readonly int port;
        private readonly IMessageHandler messageHandler;
        private readonly ClientHandler.Factory clientHandlerFactory;

        public ServerHandler(IServerSocket server, IServerEventHandler eventHandler, 
            IMessageHandler messageHandler, ClientHandler.Factory clientHandlerFactory,
            IOptions<NetworkOptions> networkOptions) : base(eventHandler)
        {
            this.port = networkOptions.Value.Port;

            this.server = server;
            this.messageHandler = messageHandler;
            this.clientHandlerFactory = clientHandlerFactory;
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

                var clientHandler = clientHandlerFactory(client, this);
                var clientThread = new Thread(() => clientHandler.Run());

                clientThread.Start();
            }
        }
    }
}
