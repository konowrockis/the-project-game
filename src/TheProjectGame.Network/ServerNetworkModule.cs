using Autofac;
using TheProjectGame.Network.Client;
using TheProjectGame.Network.Server;

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
