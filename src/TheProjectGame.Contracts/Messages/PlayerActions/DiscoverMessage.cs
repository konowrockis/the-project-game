using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(ElementName = "Discover")]
    public class DiscoverMessage : GameMessage, IMessage
    { }
}
