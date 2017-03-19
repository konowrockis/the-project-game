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
        private readonly Type eventHandler;

        public ClientNetworkModule(Type eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TcpClientSocket>().As<IClientSocket>().SingleInstance();

            builder.RegisterType(eventHandler).As<IClientEventHandler>().SingleInstance();

            builder.RegisterType<ClientHandler>().As<INetworkHandler>();
        }
    }
}
