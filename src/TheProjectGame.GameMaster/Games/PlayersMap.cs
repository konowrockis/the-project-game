
using System;
using System.Collections.Generic;
using TheProjectGame.Game;

namespace TheProjectGame.GameMaster.Games
{
    public interface IPlayersMap
    {
        string AddPlayer(GamePlayer player);
        GamePlayer GetPlayer(string guid);
    }

    public class PlayersMap : IPlayersMap
    {
        private Dictionary<string, GamePlayer> players;

        public PlayersMap()
        {
            players = new Dictionary<string, GamePlayer>();
        }

        public string AddPlayer(GamePlayer player)
        {
            string guid = Guid.NewGuid().ToString();

            players.Add(guid, player);

            return guid;
        }

        public GamePlayer GetPlayer(string guid)
        {
            GamePlayer outPlayer = null;

            players.TryGetValue(guid, out outPlayer);

            return outPlayer;
        }
    }
}
