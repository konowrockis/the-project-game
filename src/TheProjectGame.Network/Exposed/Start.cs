using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Network.Internal;

namespace TheProjectGame.Network
{
    public class Start
    {

        public static void Client(IPEndPoint endPoint, IClientEventHandler eventHandler)
        {
            ClientConnector.Connect(endPoint, eventHandler);
        }

        public static void Server(int port, IServerEventHandler eventHandler)
        {
            new ServerHandler(port, eventHandler).Run();
        }

    }
}
