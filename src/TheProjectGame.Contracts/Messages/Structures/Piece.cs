using System;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class Piece : ITimestamped
    {
        [XmlAttribute("id")]
        public ulong Id { get; set; }

        [XmlAttribute("type")]
        public PieceType Type { get; set; }

        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }

        [XmlIgnore]
        public bool PlayerIdSpecified { get; set; }
    }
}
