using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests")]
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
