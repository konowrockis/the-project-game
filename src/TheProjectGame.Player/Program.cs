﻿using System.Reflection;
using System.Threading;
using Serilog;
using Serilog.Events;
using Autofac;
using TheProjectGame.Client;
using TheProjectGame.Player.Game;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player
{
    class Program : ClientProgram<PlayerEventHandler>
    {
        protected override Assembly[] messageHandlersAssemblies => new Assembly[]
        {
            typeof(ClientProgram<>).Assembly,
            typeof(Program).Assembly
        };

        static void Main(string[] args)
        {
#if DEBUG
            Thread.Sleep(1000);
#endif

            new Program().Start();
        }

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterOptions<PlayerOptions>();
            builder.RegisterType<PlayerKnowledge>().AsSelf().SingleInstance();
            builder.RegisterType<SimplePlayerLogic>().As<IPlayerLogic>().SingleInstance();

            return base.ConfigureContainer(builder);
        }
    }
}
