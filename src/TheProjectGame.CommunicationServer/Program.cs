using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Network;

namespace TheProjectGame.CommunicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.Start.Server(20000,new ServerEventHandler());
        }

        private class ServerEventHandler : IServerEventHandler
        {
            public void OnMessage(string message, IConnection connection)
            {
                Console.WriteLine("Message from @{0}:{1} - {2}",connection.Address(),connection.Port(),message);
                connection.Close();
            }

            public void OnOpen(IConnection connection)
            {
                Console.WriteLine("New connection @{0}:{1}",connection.Address(),connection.Port());
                connection.Send("Ping");
            }

            public void OnClose(IConnectionData data)
            {
                Console.WriteLine("Connection closed @{0}:{1}", data.Address(),data.Port());
            }

            public void OnError(IConnectionData data, Exception exception)
            {
                Console.WriteLine("Client @{0}:{1} error {2}",data.Address(),data.Port(),exception);
            }

            public void OnServerStart()
            {
                Console.WriteLine("Server started");
            }

            public void OnServerError(Exception exception)
            {
                Console.WriteLine("Server error: {0}",exception.Message);
            }

            public void OnServerStop()
            {
                Console.WriteLine("Server stopped");
            }
        }

    }
}
