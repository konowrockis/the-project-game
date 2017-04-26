using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(ElementName = "AcceptExchangeRequest")]
    public class AcceptExchangeRequestMessage : BetweenPlayersMessage, IMessage
    { }
}
