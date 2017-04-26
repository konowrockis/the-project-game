using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(ElementName = "AuthorizeKnowledgeExchange")]
    public class AuthorizeKnowledgeExchangeMessage : GameMessage, IMessage
    {
        [XmlAttribute("withPlayerId")]
        public ulong WithPlayerId { get; set; }
    }
}
