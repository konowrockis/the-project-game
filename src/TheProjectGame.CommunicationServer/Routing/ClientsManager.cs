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

        public ClientsManager()
        {
            clients = new List<IClient>();
        }

        public void Add(IClient client)
        {
            clients.Add(client);
        }

        public void Remove(IClient client)
        {
            clients.Remove(client);
        }
    }
}
