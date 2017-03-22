using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TheProjectGame.Client;
using TheProjectGame.Network;

namespace TheProjectGame.Player
{
    class Program : ClientProgram<PlayerEventHandler>
    {
        protected override Assembly[] messageHandlersAssemblies => new Assembly[]
        {
            typeof(ClientProgram<>).Assembly
        };

        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(1000); // TODO: debug purpose only, remember to remove!

            new Program().Start();

            Console.ReadKey();
        }
    }
}
