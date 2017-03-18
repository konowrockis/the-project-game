using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Network.Internal.Client;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal.Server
{
    internal class TcpServerSocket : IServerSocket
    {
        private const int BACKLOG = 10;

        private Socket socket;

        public IClientSocket Accept()
        {
            return new TcpClientSocket(socket.Accept());   
        }

        public void Listen(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(localEndPoint);
            socket.Listen(BACKLOG);
        }

    }
}
