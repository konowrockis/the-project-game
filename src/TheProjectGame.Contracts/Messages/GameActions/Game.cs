using System.Collections.Generic;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class Game : PlayerMessage, IMessage
    {
        public List<Player> Players { get; set; }

        public GameBoard Board { get; set; }

        public Location PlayerLocation { get; set; }
    }
}
