using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class Player
    {
        [XmlAttribute("id")]
        public ulong Id { get; set; }

        [XmlAttribute("team")]
        public TeamColour Team { get; set; }

        [XmlAttribute("type")]
        public PlayerType Type { get; set; }
    }
}
