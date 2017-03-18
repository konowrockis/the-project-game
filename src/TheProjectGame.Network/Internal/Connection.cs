using System;
using System.Net;
using System.Threading.Tasks;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal
{
    internal class Connection : IConnection
    {
        private IClientSocket socket;
        private IMessageWriter messageHandler;

        public Connection(IClientSocket socket, IMessageWriter messageHandler)
        {
            this.socket = socket;
            this.messageHandler = messageHandler;
        }

        public void Send(string message, long delayMillis = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMillis)).ContinueWith((t) =>
            {
                try
                {
                    messageHandler.Send(socket, message);
                }
                catch { }
            });
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
