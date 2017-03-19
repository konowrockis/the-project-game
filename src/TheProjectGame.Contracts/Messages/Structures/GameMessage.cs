using System;
using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public abstract class GameMessage
    {
        [XmlAttribute("playerGuid")]
        public string PlayerGuid { get; set; }

        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
