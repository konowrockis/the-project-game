using System.Reflection;
using System.Threading;
using TheProjectGame.Client;

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
    }
}
