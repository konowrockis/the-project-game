using System;
using System.Net;
using System.Net.Sockets;
using TheProjectGame.Network.Client;

namespace TheProjectGame.Network.Server
{
    internal class TcpServerSocket : IServerSocket, IDisposable
    {
        private const int BacklogSize = 10;

        private Socket socket;

        public IClientSocket Accept()
        {
            return new TcpClientSocket(socket.Accept());   
        }

        public void Listen(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            socket.Bind(localEndPoint);
            socket.Listen(BacklogSize);
        }

        public void Dispose()
        {
            socket.Dispose();
        }
    }
}
