using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal.Client
{
    internal class TcpClientSocket : IClientSocket
    {
        private Socket socket;
        private IPEndPoint endPoint;

        public bool Connected => socket != null && socket.Connected;

        public TcpClientSocket()
        { }

        public TcpClientSocket(Socket socket)
        {
            this.socket = socket;
            this.endPoint = socket.RemoteEndPoint as IPEndPoint;
        }

        public IPAddress Address()
        {
            return this.endPoint.Address;
        }

        public void Close()
        {
            this.socket.Close();
        }

        public void Connect(IPEndPoint endPoint)
        {
            this.socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            this.socket.Connect(endPoint);
            this.endPoint = endPoint;
        }

        public int Port()
        {
            return this.endPoint.Port;
        }

        public int Read(byte[] bytes, int offset, int length)
        {
            return this.socket.Receive(bytes, offset, length, SocketFlags.None);
        }

        public int Write(byte[] bytes, int offset, int length)
        {
            return this.socket.Send(bytes, offset, length, SocketFlags.None);
        }
    }
}
