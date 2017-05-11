using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using AutoMapper;
using Serilog;
using Serilog.Events;
using TheProjectGame.Game.Builders;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Client
{
    public abstract class GameProgram<TNetworkModule> where TNetworkModule : NetworkModule, new()
    {
        protected abstract Assembly[] messageHandlersAssemblies { get; }

        public void Start()
        {
            var containerBulder = new ContainerBuilder();
            var container = ConfigureContainer(containerBulder);

            GeneralOptions options = container.Resolve<GeneralOptions>();
            InitializeLogger(container, options.Verbose);

            container.Resolve<INetworkHandler>().Run();
        }

        protected virtual IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<TNetworkModule>();
            builder.RegisterModule<MessagingModule>();

            builder.RegisterAssemblyTypes(messageHandlersAssemblies).AsClosedTypesOf(typeof(IMessageHandler<>));

            builder.RegisterType<DataBuilder>().AsSelf();

            RegisterMapper(builder);

            return builder.Build();
        }

        private void RegisterMapper(ContainerBuilder builder)
        {
            var mapperCfg = ConfigureMapper();

            mapperCfg.AssertConfigurationIsValid();
            builder.RegisterInstance(mapperCfg).AsSelf().As<IConfigurationProvider>();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper(ctx.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }

        protected abstract MapperConfiguration ConfigureMapper();

        protected void InitializeLogger(IContainer container, bool verbose = false)
        {
            LoggerConfiguration configuration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole(restrictedToMinimumLevel:
                    verbose ? LogEventLevel.Verbose : LogEventLevel.Debug)
                .WriteTo.RollingFile(@"Logs\log-{Date}.log", restrictedToMinimumLevel: LogEventLevel.Verbose);

            ConfigureLogger(container, configuration);
            Log.Logger = configuration.CreateLogger();
        }

        protected virtual void ConfigureLogger(IContainer container, LoggerConfiguration configuration)
        {

        }
    }
}
