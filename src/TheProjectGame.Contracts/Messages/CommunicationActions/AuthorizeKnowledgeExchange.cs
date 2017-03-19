using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class AuthorizeKnowledgeExchange : GameMessage, IMessage
    {
        [XmlAttribute("withPlayerId")]
        public ulong WithPlayerId { get; set; }
    }
}
