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

        protected override void ConfigureLogger(LoggerConfiguration configuration)
        {
            configuration.WriteTo.File(new CsvMessageFormatter(), "gm-log.csv",
                restrictedToMinimumLevel: LogEventLevel.Verbose);
        }

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterOptions<GameMasterOptions>();

            builder.RegisterType<CurrentGame>().As<ICurrentGame>().As<IGameCreator>().SingleInstance();

            return base.ConfigureContainer(builder);
        }
    }
}
