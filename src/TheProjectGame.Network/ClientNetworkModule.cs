using Autofac;
using TheProjectGame.Network.Client;

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
