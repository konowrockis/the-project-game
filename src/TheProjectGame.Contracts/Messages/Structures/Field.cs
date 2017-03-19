using System;
using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class Field : Location
    {
        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }

        [XmlIgnore]
        public bool PlayerIdSpecified { get; set; }
    }
}
