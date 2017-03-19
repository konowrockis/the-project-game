using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum PieceType
    {
        [XmlEnum("unknown")]
        Unknown,

        [XmlEnum("sham")]
        Sham,

        [XmlEnum("normal")]
        Normal
    }
}
