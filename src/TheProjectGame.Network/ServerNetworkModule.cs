using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Network.Internal.Server;

namespace TheProjectGame.Network
{
    public class ServerNetworkModule : NetworkModule
    {
        private Type eventHandler;

        public ServerNetworkModule(Type eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TcpServerSocket>().As<IServerSocket>().SingleInstance();

            builder.RegisterType(eventHandler).As<IServerEventHandler>().SingleInstance();

            builder.RegisterType<ServerHandler>().As<INetworkHandler>();

            builder.RegisterType<ClientHandler>();
        }
    }
}
