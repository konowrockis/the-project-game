using System.Reflection;
using System.Threading;
using Autofac;
using Serilog;
using Serilog.Events;
using TheProjectGame.Client;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.GameMaster.Games;
using AutoMapper;
using TheProjectGame.Display;

namespace TheProjectGame.GameMaster
{
    class Program : ClientProgram<GameMasterEventHandler>
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
            configuration.WriteTo.File(new CsvMessageFormatter(), "gm-log.csv",
                restrictedToMinimumLevel: LogEventLevel.Verbose);

            var options = container.Resolve<GeneralOptions>();
            if (options.Display)
            {
                GameStateDisplay.Run(container);
            }
        }

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterOptions<GameMasterOptions>();

            builder.RegisterType<CurrentGame>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ActionCostsOptions>().AsSelf().SingleInstance();

            builder.RegisterType<GameStateForm>().AsSelf();

            return base.ConfigureContainer(builder);
        }

        protected override MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.AddProfile(new ClientProfile(true));
            });
        }
    }
}
