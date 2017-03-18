using Autofac;
using TheProjectGame.Network;

namespace TheProjectGame.CommunicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureContainer();

            container.Resolve<INetworkHandler>().Run();
        }

        private static IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule(new ServerNetworkModule(new ServerEventHandler()));

            return builder.Build();
        }
    }
}
