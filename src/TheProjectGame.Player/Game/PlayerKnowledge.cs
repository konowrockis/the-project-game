using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    class PlayerKnowledge
    {
        public GamePlayer Player { get; private set; }
        public BoardPiece CarriedPiece { get; private set; }
        public List<GamePlayer> Players { get; private set; }

        public PlayerKnowledge(List<GamePlayer> players, GamePlayer player)
        {
            Player = player;
            CarriedPiece = null;
            Players = players;
        }
    }
}
