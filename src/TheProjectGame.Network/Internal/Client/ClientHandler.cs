using System;
using System.Net;
using System.Net.Sockets;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Network.Internal.Exception;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network
{
    internal class ClientHandler : INetworkHandler
    {
        private readonly IClientSocket socket;
        private readonly IClientEventHandler eventHandler;
        private readonly IConnection connection;
        private readonly Action setup;
        private readonly IMessageReader messageReader;

        public delegate ClientHandler Factory(IClientSocket socket, IClientEventHandler eventHandler);

        public ClientHandler(IOptions<NetworkOptions> networkOptions, IClientSocket socket, IClientEventHandler eventHandler, IMessageHandler messageHandler)
        {
            this.socket = socket;
            this.eventHandler = eventHandler;
            this.connection = new Connection(socket, messageHandler);
            this.messageReader = messageHandler;

            setup = () =>
            {
                if (!socket.Connected)
                { 
                    var endPoint = new IPEndPoint(IPAddress.Parse(networkOptions.Value.Address), networkOptions.Value.Port);
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
            catch (SocketClosedException)
            {
                socket.Close();
            }
            catch (ObjectDisposedException)
            {
                // ignored
            }
            catch (SocketException e)
            {
                if (e.ErrorCode != 10054 && e.ErrorCode != 10053)
                {
                    IConnectionData connData = connection;
                    eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
                }
            }
            catch (Exception e)
            {
                IConnectionData connData = connection;
                eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
            }
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
