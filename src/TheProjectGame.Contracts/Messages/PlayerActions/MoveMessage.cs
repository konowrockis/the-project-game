using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(ElementName = "Move")]
    public class MoveMessage : GameMessage, IMessage
    {
        [XmlAttribute("direction")]
        public MoveType Direction { get; set; }
    }
}
