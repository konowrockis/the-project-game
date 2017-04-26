using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "RejectJoiningGame")]
    public class RejectJoiningGameMessage : PlayerMessage, IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }
    }
}
