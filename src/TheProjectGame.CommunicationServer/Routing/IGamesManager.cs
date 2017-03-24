using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
