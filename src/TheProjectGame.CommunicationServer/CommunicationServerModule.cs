using Autofac;
using TheProjectGame.CommunicationServer.Routing;

namespace TheProjectGame.CommunicationServer
{
    class CommunicationServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServerClient>();
            builder.RegisterType<CurrentClient>().As<ICurrentClient>().InstancePerLifetimeScope();
            builder.RegisterType<ClientsManager>().As<IClientsManager>().SingleInstance();
            builder.RegisterType<GamesManager>().As<IGamesManager>().SingleInstance();
        }
    }
}
