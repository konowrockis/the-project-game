using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "JoinGame")]
    public class JoinGameMessage : IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }

        [XmlAttribute("preferredTeam")]
        public TeamColor PreferedTeam { get; set; }

        [XmlAttribute("preferredRole")]
        public PlayerType PreferedRole { get; set; }

        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }

        [XmlIgnore]
        public bool PlayerIdSpecified { get; set; }
    }
}
