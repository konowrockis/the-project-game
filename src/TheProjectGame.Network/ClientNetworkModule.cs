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
    public class ClientNetworkModule<TEventHandler> : NetworkModule where TEventHandler: IClientEventHandler
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TcpClientSocket>().As<IClientSocket>().InstancePerLifetimeScope();

            builder.RegisterType<TEventHandler>().As<IClientEventHandler>().InstancePerLifetimeScope();

            builder.RegisterType<ClientHandler>().As<INetworkHandler>().InstancePerLifetimeScope();
        }
    }
}
