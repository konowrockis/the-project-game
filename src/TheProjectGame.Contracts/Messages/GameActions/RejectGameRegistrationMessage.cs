using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "RejectGameRegistration")]
    public class RejectGameRegistrationMessage : IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }
    }
}
