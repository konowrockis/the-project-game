using System.Reflection;
using System.Threading;
using Serilog;
using Serilog.Events;
using TheProjectGame.Client;

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
    }
}
