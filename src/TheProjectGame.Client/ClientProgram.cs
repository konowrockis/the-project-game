using System;
using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings;

namespace TheProjectGame.Client
{
    public abstract class ClientProgram<T> where T: IClientEventHandler
    {
        protected abstract Assembly[] messageHandlersAssemblies { get; }

        protected void Start()
        {
            var container = ConfigureContainer();

            container.Resolve<INetworkHandler>().Run();
        }

        private IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new ClientNetworkModule(typeof(T)));
            builder.RegisterModule(new MessagingModule());

            builder.RegisterOptions<Settings.Options.NetworkOptions>();

            builder.RegisterAssemblyTypes(messageHandlersAssemblies).AsClosedTypesOf(typeof(IMessageHandler<>));

            return builder.Build();
        }
    }
}
