using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class JoinGame : IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }

        [XmlAttribute("preferedTeam")]
        public TeamColor PreferedTeam { get; set; }

        [XmlAttribute("preferedRole")]
        public PlayerType PreferedRole { get; set; }

        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }

        [XmlIgnore]
        public bool PlayerIdSpecified { get; set; }
    }
}
