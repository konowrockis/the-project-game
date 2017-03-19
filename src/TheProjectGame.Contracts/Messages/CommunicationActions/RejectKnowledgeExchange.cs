using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class RejectKnowledgeExchange : BetweenPlayersMessage, IMessage
    {
        [XmlAttribute("Permanent")]
        public bool Permanent { get; set; }
    }
}
