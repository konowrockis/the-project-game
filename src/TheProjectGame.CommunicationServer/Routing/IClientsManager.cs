using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.CommunicationServer.Routing
{
    interface IClientsManager
    {
        void Add(IClient client);
        void Remove(IClient client);

        ulong GetNewPlayerId();

        IClient GetPlayerById(ulong id);
    }
}
