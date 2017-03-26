using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests")]
namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class JoinGame : IMessage
    {
        [XmlAttribute("gameName")]
        public string GameName { get; set; }

        [XmlAttribute("preferedTeam")]
        public TeamColour PreferedTeam { get; set; }

        [XmlAttribute("preferedRole")]
        public PlayerType PreferedRole { get; set; }

        [XmlAttribute("playerId")]
        public ulong PlayerId { get; set; }

        [XmlIgnore]
        public bool PlayerIdSpecified { get; set; }
    }
}
