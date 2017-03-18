using System;
using System.Net;
using System.Net.Sockets;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network
{
    internal class ClientHandler
    {
        private readonly IClientSocket socket;
        private readonly IClientEventHandler eventHandler;
        private readonly IConnection connection;
        private readonly Action setup;
        private readonly IMessageReader messageReader;

        public ClientHandler(IPEndPoint endPoint, IClientSocket socket, IClientEventHandler eventHandler, IMessageHandler messageHandler)
        {
            this.socket = socket;
            this.eventHandler = eventHandler;
            this.connection = new Connection(socket, messageHandler);
            this.messageReader = messageHandler;
            setup = () => { socket.Connect(endPoint); };
        }
        public ClientHandler(IClientSocket socket, IClientEventHandler eventHandler, IMessageHandler messageHandler)
        {
            this.socket = socket;
            this.eventHandler = eventHandler;
            this.connection = new Connection(socket, messageHandler);
            this.messageReader = messageHandler;
            setup = () => { };
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
                when ( !(e is ObjectDisposedException || (e is SocketException && ((SocketException)e).ErrorCode == 10054)) )
            {
                IConnectionData connData = connection;
                eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
            }
            catch (Exception e)
            {
                // ignored
            }
            if (opened) eventHandler.OnClose(connection);
        }

        private void Listen()
        {
            string msg = messageReader.Read(socket);
            eventHandler.OnMessage(connection, msg);
        }
    }
}
