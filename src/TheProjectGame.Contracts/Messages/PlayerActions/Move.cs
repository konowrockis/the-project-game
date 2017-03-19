using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class Move : GameMessage, IMessage
    {
        [XmlAttribute("direction")]
        public MoveType Direction { get; set; }
    }
}
