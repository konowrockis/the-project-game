using System.Reflection;
using System.Threading;
using Autofac;
using TheProjectGame.Client;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

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

        protected override IContainer ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterOptions<GameMasterOptions>();

            return base.ConfigureContainer(builder);
        }
    }
}
