using System.Net;
using System.Net.Sockets;

namespace TheProjectGame.Network.Client
{
    class TcpClientSocket : IClientSocket
    {
        public Socket RawSocket { get; private set; }

        private IPEndPoint endPoint;

        public bool Connected => RawSocket != null && RawSocket.Connected;

        public int Port => endPoint.Port;

        public IPAddress Address => endPoint.Address;

        public TcpClientSocket()
        { }

        public TcpClientSocket(Socket socket)
        {
            RawSocket = socket;

            endPoint = socket.RemoteEndPoint as IPEndPoint;
        }

        public void Close()
        {
            RawSocket.Close();
        }

        public void Connect(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;

            RawSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            RawSocket.Connect(endPoint);
        }
    }
}
