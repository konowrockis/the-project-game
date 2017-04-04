using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class RejectJoiningGame : PlayerMessage, IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }
    }
}
