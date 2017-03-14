using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal
{
    internal class ClientConnector
    {

        public static void Connect(IPEndPoint endPoint, IClientEventHandler eventHandler)
        {
            try
            {
                Socket socket = DoConnect(endPoint);
                new ClientHandler(socket, eventHandler).Run();
            }
            catch (Exception e)
            {
                eventHandler.OnError(new NullConnectionData(),e);
            }
        }

        private static Socket DoConnect(IPEndPoint endPoint)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            return socket;
        }

    }
}
