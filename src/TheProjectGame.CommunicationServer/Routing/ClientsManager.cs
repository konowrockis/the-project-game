﻿using System.Collections.Concurrent;

namespace TheProjectGame.CommunicationServer.Routing
{
    class ClientsManager : IClientsManager
    {
        private readonly ConcurrentDictionary<ulong, IClient> clients;
        private ulong newPlayerId;

        private object newPlayerIdLockObject = new object();

        public ClientsManager()
        {
            clients = new ConcurrentDictionary<ulong, IClient>();
            newPlayerId = 1;
        }

        public void Add(IClient client)
        {
            clients.TryAdd(client.PlayerId, client);
        }

        public ulong GetNewPlayerId()
        {
            lock (newPlayerIdLockObject)
            {
                return newPlayerId++;
            }
        }

        public IClient GetPlayerById(ulong id)
        {
            if (clients.TryGetValue(id, out IClient outClient))
            {
                return outClient;
            }
            return null;
        }

        public void Remove(IClient client)
        {
            clients.TryRemove(client.PlayerId, out IClient outClient);
        }
    }
}
