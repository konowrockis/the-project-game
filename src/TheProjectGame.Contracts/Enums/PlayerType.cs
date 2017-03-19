using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum PlayerType
    {
        [XmlEnum("leader")]
        Leader,

        [XmlEnum("player")]
        Player
    }
}
