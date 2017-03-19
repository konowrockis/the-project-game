using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.CommunicationActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class KnowledgeExchangeRequest : BetweenPlayersMessage, IMessage
    { }
}
