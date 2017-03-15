using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    internal class ClientHandler
    {
        private readonly Socket socket;
        private readonly IClientEventHandler eventHandler;
        private readonly IConnection connection;

        public ClientHandler(Socket socket, IClientEventHandler eventHandler)
        {
            this.socket = socket;
            this.eventHandler = eventHandler;
            this.connection = new Connection(socket);
        }

        public void Run()
        {
            eventHandler.OnOpen(connection);
            try
            {
                Listen();
            }
            catch (Exception e)
            {
                if ((!(e is System.ObjectDisposedException)))
                    eventHandler.OnError(connection, e);
            }
            eventHandler.OnClose(connection);
        }

        private void Listen()
        {
            MemoryStream buffer = new MemoryStream();
            byte[] b = new byte[1];
            while (socket.Receive(b, 0, 1, SocketFlags.None) > 0)
            {
                if (b[0] != Utils.ETB)
                {
                    buffer.Write(b, 0, 1);
                }
                else
                {
                    string msg = Utils.BytesToString(buffer.ToArray());
                    buffer.SetLength(0);
                    eventHandler.OnMessage(msg, connection);
                }
            }
        }
    }
}
