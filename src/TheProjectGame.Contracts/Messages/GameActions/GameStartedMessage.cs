using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "Game")]
    public class GameStartedMessage : PlayerMessage, IMessage
    {
        public List<Player> Players { get; set; }

        public GameBoard Board { get; set; }

        public Location PlayerLocation { get; set; }
    }
}
