using System.Net;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Client;
using TheProjectGame.Network.Internal.Server;

namespace TheProjectGame.Network
{
    public class Start
    {
        public static void Client(IPEndPoint endPoint, IClientEventHandler eventHandler)
        {
            new ClientHandler(endPoint, new TcpClientSocket(), eventHandler,new MessageHandler()).Run();
        }

        public static void Server(int port, IServerEventHandler eventHandler)
        {
            new ServerHandler(port, new TcpServerSocket(), eventHandler, new MessageHandler()).Run();
        }

    }
}
