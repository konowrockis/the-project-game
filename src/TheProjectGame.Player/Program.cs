using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TheProjectGame.Client;
using TheProjectGame.Network;

namespace TheProjectGame.Player
{
    class Program : ClientProgram
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        protected override IClientEventHandler GetClientEventHandler()
        {
            return new PlayerEventHandler();
        }
    }
}
