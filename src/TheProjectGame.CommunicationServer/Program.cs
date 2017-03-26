using System.Reflection;
using Autofac;
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
            typeof(Program).Assembly
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
    }
}
