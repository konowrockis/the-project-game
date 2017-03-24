using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class PlayerMessage : IMessage
    {
        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }
    }
}
