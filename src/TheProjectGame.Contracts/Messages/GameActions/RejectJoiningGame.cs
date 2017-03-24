using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class RejectJoiningGame : PlayerMessage, IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }
    }
}
