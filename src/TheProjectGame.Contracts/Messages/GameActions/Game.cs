using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class Game : PlayerMessage, IMessage
    {
        public List<Player> Players { get; set; }

        public GameBoard Board { get; set; }

        public Location PlayerLocation { get; set; }
    }
}
