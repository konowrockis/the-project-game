using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal.Client
{
    internal class TcpClientSocket : IClientSocket
    {
        public Socket RawSocket { get; private set; }
        private IPEndPoint endPoint;

        public bool Connected => RawSocket != null && RawSocket.Connected;

        public TcpClientSocket()
        { }

        public TcpClientSocket(Socket socket)
        {
            RawSocket = socket;
            this.endPoint = socket.RemoteEndPoint as IPEndPoint;
        }

        public IPAddress Address()
        {
            return this.endPoint.Address;
        }

        public void Close()
        {
            this.RawSocket.Close();
        }

        public void Connect(IPEndPoint endPoint)
        {
            RawSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            RawSocket.Connect(endPoint);
            
            this.endPoint = endPoint;
        }

        public int Port()
        {
            return this.endPoint.Port;
        }
    }
}
