using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Xml;
using TheProjectGame.Client;

namespace TheProjectGame.GameMaster
{
    class Program : ClientProgram<GameMasterEventHandler>
    {
        protected override Assembly[] messageHandlersAssemblies => new Assembly[]
        {
            typeof(ClientProgram<>).Assembly
        };

        static void Main(string[] args)
        {
            new Program().Start();
        }
    }
}
