using System;
using System.Net;
using System.Threading.Tasks;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal
{
    internal class Connection : IConnection
    {
        private IClientSocket socket;

        public Connection(IClientSocket socket)
        {
            this.socket = socket;
        }

        public void Close()
        {
            socket.Close();
        }

        public IPAddress Address()
        {
            return socket.Address();
        }

        public int Port()
        {
            return socket.Port();
        }

        public bool Connected => socket.Connected;
    }
}
