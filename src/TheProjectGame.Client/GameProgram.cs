﻿using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings;

namespace TheProjectGame.Client
{
    public abstract class GameProgram<TNetworkModule> where TNetworkModule: NetworkModule, new()
    {
        protected abstract Assembly[] messageHandlersAssemblies { get; }

        protected void Start()
        {
            var containerBulder = new ContainerBuilder();
            var container = ConfigureContainer(containerBulder);

            container.Resolve<INetworkHandler>().Run();
        }

        protected virtual IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<TNetworkModule>();
            builder.RegisterModule<MessagingModule>();

            builder.RegisterAssemblyTypes(messageHandlersAssemblies).AsClosedTypesOf(typeof(IMessageHandler<>)).InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
