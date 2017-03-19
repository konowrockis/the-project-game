using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    public abstract class BetweenPlayersMessage : PlayerMessage
    {
        [XmlAttribute("senderPlayerId")]
        public ulong SenderPlayerId { get; set; }
    }
}
