using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.Routing
{
    class ClientsManager : IClientsManager
    {
        private readonly List<IClient> clients; // TODO: make this thread safe maybe?
        private ulong newPlayerId;

        public ClientsManager()
        {
            clients = new List<IClient>();
            newPlayerId = 1;
        }

        public void Add(IClient client)
        {
            clients.Add(client);
        }

        public ulong GetNewPlayerId() => newPlayerId++;

        public IClient GetPlayerById(ulong id)
        {
            return clients.FirstOrDefault(p => p.PlayerId == id);
        }

        public void Remove(IClient client)
        {
            clients.Remove(client);
        }
    }
}
