using System.Xml.Serialization;

namespace TheProjectGame.Settings.Options.Structures
{
    public enum GoalFieldType
    {
        [XmlEnum("goal")]
        Goal,

        [XmlEnum("non-goal")]
        NonGoal
    }

    public enum TeamColor
    {
        [XmlEnum("red")]
        Red,

        [XmlEnum("blue")]
        Blue
    }

    public class GoalField
    {
        [XmlAttribute("x")]
        public uint X { get; set; }

        [XmlAttribute("y")]
        public uint Y { get; set; }
        
        [XmlAttribute("type")]
        public GoalFieldType Type { get; set; }

        [XmlAttribute("team")]
        public TeamColor Team { get; set; }
    }
}
