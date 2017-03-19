using System.Xml.Serialization;

namespace TheProjectGame.Contracts
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class GameFinished : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
