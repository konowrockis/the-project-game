using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TheProjectGame.Network;
using TheProjectGame.Settings;

namespace TheProjectGame.Client
{
    public abstract class ClientProgram
    {
        protected void Start()
        {
            var container = ConfigureContainer();

            container.Resolve<INetworkHandler>().Run();
        }

        protected abstract IClientEventHandler GetClientEventHandler();

        private IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ClientNetworkModule(GetClientEventHandler()));

            builder.RegisterOptions<Settings.Options.NetworkOptions>();
            
            return builder.Build();
        }
    }
}
