using System.Net;

namespace TheProjectGame.Network.Client
{
    internal class Connection : IConnection
    {
        private IClientSocket socket;

        public IPAddress Address => socket.Address;

        public int Port => socket.Port;

        public bool Connected => socket.Connected;

        public Connection(IClientSocket socket)
        {
            this.socket = socket;
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
