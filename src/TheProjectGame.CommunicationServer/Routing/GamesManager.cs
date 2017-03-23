using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.CommunicationServer.Routing
{
    class GamesManager : IGamesManager
    {
        private ulong nextGameId;
        private IList<IGame> games;

        public GamesManager()
        {
            games = new List<IGame>();
            nextGameId = 1;
        }

        public void Add(IGame game)
        {
            games.Add(game);
        }

        public ulong GetNewGameId() => nextGameId++;

        public void Remove(IGame game)
        {
            games.Remove(game);
        }

        public IGame GetGameByName(string name)
        {
            return games.FirstOrDefault(g => g.Name == name);
        }

        public IGame GetGameById(ulong id)
        {
            return games.FirstOrDefault(g => g.Id == id);
        }

        public IReadOnlyList<IGame> GetGamesList()
        {
            return games.ToList();
        }
    }
}
