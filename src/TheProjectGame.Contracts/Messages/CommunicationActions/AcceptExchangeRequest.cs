using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class AcceptExchangeRequest : BetweenPlayersMessage, IMessage
    { }
}
