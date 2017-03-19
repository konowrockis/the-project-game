using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class Data : PlayerMessage, IMessage
    {
        public List<TaskField> TaskFields { get; set; }

        public List<GoalField> GoalFields { get; set; }

        public List<Piece> Pieces { get; set; }

        public Location PlayerLocation { get; set; }

        [XmlAttribute("gameFinished")]
        public bool GameFinished { get; set; }
    }
}
