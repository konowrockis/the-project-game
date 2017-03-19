using Autofac;
using Autofac.Features.Variance;
using TheProjectGame.Messaging;
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

            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ServerNetworkModule(typeof(ServerEventHandler)));
            builder.RegisterModule(new MessagingModule());

            builder.RegisterOptions<Settings.Options.NetworkOptions>();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsClosedTypesOf(typeof(IMessageHandler<>)).InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
