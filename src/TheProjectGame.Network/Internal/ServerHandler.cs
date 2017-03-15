using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal
{
    internal class ServerHandler
    {
        private const int BACKLOG = 10;
        private int serverPort;
        private IServerEventHandler eventHandler;
        private Dictionary<string, Socket> socketDictionary;

        private Object mtx =new Object();

        public ServerHandler(int serverPort, IServerEventHandler eventHandler)
        {
            this.serverPort = serverPort;
            this.eventHandler = eventHandler;
            this.socketDictionary = new Dictionary<string, Socket>();
        }

        public void Run()
        {
            eventHandler.OnServerStart();
            try
            {
                Work();
            }
            catch (Exception e)
            {
                eventHandler.OnServerError(e);
            }
            eventHandler.OnServerStop();
        }

        private void Work()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(BACKLOG);

            while (true)
            {
                Socket socket = listener.Accept();
                
                var clientHandler = new ClientHandler(socket, eventHandler);
                var clientThread = new Thread(() => clientHandler.Run());

                lock (mtx)
                {
                    socketDictionary.Add(GetId(socket),socket);
                }

                clientThread.Start();
            }
        }

        private string GetId(Socket socket)
        {
            IPEndPoint endpoint = socket.RemoteEndPoint as IPEndPoint;
            int port = endpoint.Port;
            IPAddress address = endpoint.Address;
            return GetId(address, port);
        }

        private string GetId(IPAddress address,int port)
        {
            return $"{address}:{port}";
        }

        private class ServerEventHandlerWrapper :IServerEventHandler
        {
            private IServerEventHandler eventHandler;
            private ServerHandler handler;

            public ServerEventHandlerWrapper(IServerEventHandler eventHandler, ServerHandler handler)
            {
                this.eventHandler = eventHandler;
                this.handler = handler;
            }

            public void OnMessage(string message, IConnection connection)
            {
                lock (handler.mtx)
                {
                    eventHandler.OnMessage(message, connection);
                }
            }

            public void OnOpen(IConnection connection)
            {
                lock (handler.mtx)
                {
                    eventHandler.OnOpen(connection);
                }
            }

            public void OnClose(IConnectionData data)
            {
                lock (handler.mtx)
                {
                    handler.socketDictionary.Remove(handler.GetId(data.Address(), data.Port()));
                    eventHandler.OnClose(data);
                }   
            }

            public void OnError(IConnectionData data, Exception exception)
            {
                lock (handler.mtx)
                {
                    eventHandler.OnError(data, exception);
                }
            }

            public void OnServerStart()
            {
                lock (handler.mtx)
                {
                    eventHandler.OnServerStart();
                }
            }

            public void OnServerError(Exception exception)
            {
                lock (handler.mtx)
                {
                    eventHandler.OnServerError(exception);
                }
            }

            public void OnServerStop()
            {
                lock (handler.mtx)
                {
                    eventHandler.OnServerStop();
                }
            }
        }

    }
}
