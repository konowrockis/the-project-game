using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class RejectGameRegistration : IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }
    }
}
