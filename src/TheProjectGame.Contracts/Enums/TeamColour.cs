using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum TeamColour
    {
        [XmlEnum("red")]
        Red,

        [XmlEnum("blue")]
        Blue
    }
}
