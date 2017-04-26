using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "GetGames")]
    public class GetGamesMessage : IMessage
    { }
}
