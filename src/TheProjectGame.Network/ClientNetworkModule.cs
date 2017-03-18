using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using TheProjectGame.Network.Internal.Client;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network
{
    public class ClientNetworkModule : NetworkModule
    {
        private readonly IClientEventHandler eventHandler;

        public ClientNetworkModule(IClientEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TcpClientSocket>().As<IClientSocket>().SingleInstance();

            builder.RegisterInstance(eventHandler).As<IClientEventHandler>();

            builder.RegisterType<ClientHandler>().As<INetworkHandler>();

            builder.RegisterInstance(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000)).As<IPEndPoint>();
        }
    }
}
