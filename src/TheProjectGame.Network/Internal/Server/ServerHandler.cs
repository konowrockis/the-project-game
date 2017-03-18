using System;
using System.Net;
using System.Threading;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal.Server
{
    internal class ServerHandler : SynchronousServerEventWrapper
    {
        private readonly IServerSocket server;
        private readonly int port;
        private readonly IMessageHandler messageHandler;

        public ServerHandler(int port, IServerSocket server, IServerEventHandler eventHandler, IMessageHandler messageHandler) : base(eventHandler)
        {
            this.port = port;
            this.server = server;
            this.messageHandler = messageHandler;
        }

        public void Run()
        {
            this.OnServerStart();
            try
            {
                Work();
            }
            catch (Exception e)
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
                
                var clientHandler = new ClientHandler(client, this, messageHandler);
                var clientThread = new Thread(() => clientHandler.Run());
                

                clientThread.Start();
            }
        }
    }
}
