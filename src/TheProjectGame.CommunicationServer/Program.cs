using System.Reflection;
using Autofac;
using AutoMapper;
using TheProjectGame.Client;
using TheProjectGame.Network;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.CommunicationServer
{
    class Program : GameProgram<ServerNetworkModule<ServerEventHandler>>
    {
        protected override Assembly[] messageHandlersAssemblies => new Assembly[]
        {
            typeof(Program).Assembly, typeof(GameProgram<>).Assembly
        };

        static void Main(string[] args)
        {
            new Program().Start();
        }

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CommunicationServerModule());

            builder.RegisterOptions<CommunicationServerOptions>();

            return base.ConfigureContainer(builder);
        }

        protected override MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg => { });
        }
    }
}
