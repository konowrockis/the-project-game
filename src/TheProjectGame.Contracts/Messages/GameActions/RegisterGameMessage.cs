using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "RegisterGame")]
    public class RegisterGameMessage : IMessage
    {
        public GameInfo NewGameInfo { get; set; }
    }
}
