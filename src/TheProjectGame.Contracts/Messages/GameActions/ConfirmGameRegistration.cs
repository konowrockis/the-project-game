using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class ConfirmGameRegistration : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
