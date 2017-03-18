using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Network;

namespace TheProjectGame.Player
{
    class PlayerEventHandler : IClientEventHandler
    {
        public void OnMessage(IConnection connection, string message)
        {
            Console.WriteLine("Received message: {0}", message);
            connection.Send("Pong");
        }

        public void OnOpen(IConnection connection)
        {
            Console.WriteLine("Connected");
        }

        public void OnClose(IConnectionData data)
        {
            Console.WriteLine("Disconnected");
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            Console.WriteLine("Error = {0}", exception);
        }
    }
}
