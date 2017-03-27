using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests"),InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace TheProjectGame.CommunicationServer.Routing
{
    interface IGamesManager
    {
        void Add(IGame game);
        void Remove(IGame game);

        ulong GetNewGameId();

        IGame GetGameByName(string name);
        IGame GetGameById(ulong id);
        IReadOnlyList<IGame> GetGamesList();
    }
}
