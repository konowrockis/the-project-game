using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum MoveType
    {
        [XmlEnum("up")]
        Up,

        [XmlEnum("down")]
        Down,

        [XmlEnum("left")]
        Left,

        [XmlEnum("right")]
        Right
    }
}
