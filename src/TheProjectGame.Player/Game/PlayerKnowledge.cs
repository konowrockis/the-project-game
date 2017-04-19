using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public class PlayerKnowledge
    {
        public GamePlayer Player { get; set; }
        public BoardPiece CarriedPiece { get; set; }
        public string Guid { get; set; }
        public IGameState GameState { get; set; }

        public PlayerKnowledge()
        {

        }
    }
}
