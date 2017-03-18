using Autofac;
using TheProjectGame.Network;
using TheProjectGame.Settings;

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

            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ServerNetworkModule(new ServerEventHandler()));

            builder.RegisterOptions<Settings.Options.NetworkOptions>();

            return builder.Build();
        }
    }
}
