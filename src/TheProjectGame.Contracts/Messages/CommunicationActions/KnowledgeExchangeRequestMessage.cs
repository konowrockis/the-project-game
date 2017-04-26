using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(ElementName = "KnowledgeExchangeRequest")]
    public class KnowledgeExchangeRequestMessage : BetweenPlayersMessage, IMessage
    { }
}
