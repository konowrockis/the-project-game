using System.Xml.Serialization;

namespace TheProjectGame.Contracts
{
    [XmlRoot(ElementName = "GameFinished")]
    public class GameFinished : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
