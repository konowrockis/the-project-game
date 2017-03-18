using System;
using System.Net;
using System.Net.Sockets;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network
{
    internal class ClientHandler : INetworkHandler
    {
        private readonly IClientSocket socket;
        private readonly IClientEventHandler eventHandler;
        private readonly IConnection connection;
        private readonly Action setup;
        private readonly IMessageReader messageReader;

        public delegate ClientHandler Factory(IPEndPoint endPoint, IClientSocket socket, IClientEventHandler eventHandler);

        public ClientHandler(IPEndPoint endPoint, IClientSocket socket, IClientEventHandler eventHandler, IMessageHandler messageHandler)
        {
            this.socket = socket;
            this.eventHandler = eventHandler;
            this.connection = new Connection(socket, messageHandler);
            this.messageReader = messageHandler;

            setup = () =>
            {
                if (endPoint != null)
                {
                    socket.Connect(endPoint);
                }
            };
        }

        public void Run()
        {
            bool opened = false;
            try
            {
                setup();
                opened = true;

                eventHandler.OnOpen(connection);

                Listen();
            }
            catch (Exception e)
                when (!(e is ObjectDisposedException || (e is SocketException && ((SocketException)e).ErrorCode == 10054)))
            {
                IConnectionData connData = connection;
                eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
            }
            catch (Exception) { } // ignored
            finally
            {
                if (opened) eventHandler.OnClose(connection);
            }
        }

        private void Listen()
        {
            while (true)
            {
                string msg = messageReader.Read(socket);
                eventHandler.OnMessage(connection, msg);
            }
        }
    }
}
