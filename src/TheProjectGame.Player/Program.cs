using System.Reflection;
using System.Threading;
using Serilog;
using Autofac;
using TheProjectGame.Client;
using TheProjectGame.Player.Game;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;
using AutoMapper;
using TheProjectGame.Game;
using TheProjectGame.Display;

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

        protected override void ConfigureLogger(IContainer container, LoggerConfiguration configuration)
        {
            base.ConfigureLogger(container, configuration);

            var options = container.Resolve<GeneralOptions>();
            if (options.Display)
            {
                GameStateDisplay.Run(container);
            }
        }

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterOptions<PlayerOptions>();
            builder.RegisterType<PlayerKnowledge>().As<IPlayerKnowledge>().As<IGameHolder>().SingleInstance();
            builder.RegisterType<SimplePlayerLogic>().As<IPlayerLogic>().SingleInstance();

            builder.RegisterType<GameStateForm>().AsSelf();

            return base.ConfigureContainer(builder);
        }

        protected override MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile(new ClientProfile(false));
            });
        }
    }
}
