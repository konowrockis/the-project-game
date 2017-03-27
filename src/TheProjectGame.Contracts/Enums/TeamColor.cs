using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum TeamColor
    {
        [XmlEnum("red")]
        Red,

        [XmlEnum("blue")]
        Blue
    }
}
