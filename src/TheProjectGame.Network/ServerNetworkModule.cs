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
    public class ServerNetworkModule<TEventHandler> : NetworkModule
        where TEventHandler: IClientEventHandler
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TcpServerSocket>().As<IServerSocket>().SingleInstance();

            builder.RegisterType<TEventHandler>().As<IClientEventHandler>().InstancePerLifetimeScope();

            builder.RegisterType<ServerHandler>().As<INetworkHandler>().SingleInstance();

            builder.RegisterType<ClientHandler>();
        }
    }
}
