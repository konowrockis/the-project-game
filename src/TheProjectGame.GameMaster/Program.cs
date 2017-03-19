using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using TheProjectGame.Client;

namespace TheProjectGame.GameMaster
{
    class Program : ClientProgram<GameMasterEventHandler>
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }
    }
}
