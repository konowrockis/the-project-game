using System;
using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class GameInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("redTeamPlayers")]
        public ulong RedTeamPlayers { get; set; }

        [XmlAttribute("blueTeamPlayers")]
        public ulong BlueTeamPlayers { get; set; }
    }
}
