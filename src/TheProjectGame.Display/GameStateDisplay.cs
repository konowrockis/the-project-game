using Autofac;
using System.Threading;
using System.Windows.Forms;

namespace TheProjectGame.Display
{
    public static class GameStateDisplay
    {
        public static void Run(ILifetimeScope lifetimeScope)
        {
            var form = lifetimeScope.Resolve<GameStateForm>();

            new Thread(new ThreadStart(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(form);
            })).Start();
        }
    }
}
