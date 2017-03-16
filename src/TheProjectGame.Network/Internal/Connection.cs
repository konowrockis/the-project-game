using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    internal class Connection : IConnection
    {
        private Socket socket;
        private bool closed = false;
        private IPAddress address;
        private int port;

        public Connection(Socket socket)
        {
            this.socket = socket;
            IPEndPoint endpoint = socket.RemoteEndPoint as IPEndPoint;
            if (endpoint == null) return;
            port = endpoint.Port;
            address = endpoint.Address;
        }

        public void Send(string message, long delayMillis = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMillis)).ContinueWith((t) =>
            {
                DoSend(message);
            });
        }

        private void DoSend(string message)
        {
            if (closed) return;
            byte[] msgBytes = Utils.StringToBytes(message);
            byte[] data = new byte[msgBytes.Length + 1];
            Array.Copy(msgBytes, 0, data, 0, msgBytes.Length);
            data[msgBytes.Length] = Utils.ETB;
            int wrote = 0;
            while (wrote < data.Length)
            {
                wrote += socket.Send(data, wrote, data.Length - wrote, SocketFlags.None);
            }
        }

        public void Close()
        {
            if (closed) return;
            socket.Close();
            closed = true;
        }

        public IPAddress Address()
        {
            return address;
        }

        public int Port()
        {
            return port;
        }
    }
}
