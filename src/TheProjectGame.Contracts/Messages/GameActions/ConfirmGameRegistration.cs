using System.Runtime.CompilerServices;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests")]
namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class ConfirmGameRegistration : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
