using System.Collections.Generic;

namespace TheProjectGame.CommunicationServer.Routing
{
    public interface IGamesManager
    {
        void Add(IGame game);
        void Remove(IGame game);

        ulong GetNewGameId();

        IGame GetGameByName(string name);
        IGame GetGameById(ulong id);
        IReadOnlyList<IGame> GetGamesList();
    }
}
