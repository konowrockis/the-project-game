using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    public class RejectKnowledgeExchange : BetweenPlayersMessage, IMessage
    {
        [XmlAttribute("Permanent")]
        public bool Permanent { get; set; }
    }
}
