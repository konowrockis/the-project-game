using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using Serilog;
using Serilog.Events;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
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
            //container.Resolve<GameOptions>().Verbose
            InitializeLogger(true);
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

        protected void InitializeLogger(bool verbose=false)
        {
            LoggerConfiguration configuration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole(restrictedToMinimumLevel:
                    verbose ? LogEventLevel.Verbose : LogEventLevel.Information)
                .WriteTo.RollingFile("log-{Date}.txt", restrictedToMinimumLevel: LogEventLevel.Verbose);
            ConfigureLogger(configuration);
            Log.Logger = configuration.CreateLogger();
        }

        protected virtual void ConfigureLogger(LoggerConfiguration configuration)
        {

        }
    }
}
