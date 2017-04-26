using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(ElementName = "RejectKnowledgeExchange")]
    public class RejectKnowledgeExchangeMessage : BetweenPlayersMessage, IMessage
    {
        [XmlAttribute("Permanent")]
        public bool Permanent { get; set; }
    }
}
