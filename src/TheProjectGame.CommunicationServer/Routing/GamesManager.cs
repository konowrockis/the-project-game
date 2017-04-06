﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TheProjectGame.CommunicationServer.Routing
{
    class GamesManager : IGamesManager
    {
        private ulong nextGameId;
        private readonly ConcurrentDictionary<ulong, IGame> games;

        private object nextGameIdLockObject = new object();

        public GamesManager()
        {
            games = new ConcurrentDictionary<ulong, IGame>();
            nextGameId = 1;
        }

        public void Add(IGame game)
        {
            games.TryAdd(game.Id, game);
        }

        public ulong GetNewGameId()
        {
            lock (nextGameIdLockObject)
            {
                return nextGameId++;
            }
        }

        public void Remove(IGame game)
        {
            games.TryRemove(game.Id, out IGame outGame);
        }

        public IGame GetGameByName(string name)
        {
            return games.FirstOrDefault(g => g.Value.Name == name).Value;
        }

        public IGame GetGameById(ulong id)
        {
            if (games.TryGetValue(id, out IGame outGame))
            {
                return outGame;
            }
            return null;
        }

        public IReadOnlyList<IGame> GetGamesList()
        {
            return games.Values.ToList();
        }
    }
}
