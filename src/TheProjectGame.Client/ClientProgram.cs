using Autofac;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings;

namespace TheProjectGame.Client
{
    public abstract class ClientProgram<T> where T: IClientEventHandler
    {
        protected void Start()
        {
            var container = ConfigureContainer();

            container.Resolve<INetworkHandler>().Run();
        }

        private IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ClientNetworkModule(typeof(T)));
            builder.RegisterModule(new MessagingModule());

            builder.RegisterOptions<Settings.Options.NetworkOptions>();
            
            return builder.Build();
        }
    }
}
