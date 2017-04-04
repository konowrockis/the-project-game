using System.Xml.Serialization;

namespace TheProjectGame.Contracts
{
    public class GameFinished : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
