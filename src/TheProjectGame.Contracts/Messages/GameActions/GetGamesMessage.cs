using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot("GetGames")]
    public class GetGamesMessage : IMessage
    { }
}
