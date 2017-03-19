using System;
using System.IO;
using System.Xml;
using TheProjectGame.Network;

namespace TheProjectGame.GameMaster
{
    class GameMasterEventHandler : IClientEventHandler
    {
        public void OnOpen(IConnection connection, Stream stream)
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
