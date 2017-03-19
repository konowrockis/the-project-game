using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Enums
{
    public enum GoalFieldType
    {
        [XmlEnum("unknown")]
        Unknown,

        [XmlEnum("goal")]
        Goal,

        [XmlEnum("non-goal")]
        NonGoal
    }
}
