using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "ConfirmGameRegistration")]
    public class ConfirmGameRegistrationMessage : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
