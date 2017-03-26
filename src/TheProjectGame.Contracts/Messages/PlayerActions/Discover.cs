using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests")]
namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class Discover : GameMessage, IMessage
    { }
}
