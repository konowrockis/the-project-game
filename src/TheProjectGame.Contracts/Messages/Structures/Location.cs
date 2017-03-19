using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class Location
    {
        [XmlAttribute("x")]
        public uint X { get; set; }

        [XmlAttribute("y")]
        public uint Y { get; set; }
    }
}
