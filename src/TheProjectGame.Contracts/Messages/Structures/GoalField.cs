using System;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class GoalField : Field
    {
        [XmlAttribute("type")]
        public GoalFieldType Type { get; set; }

        [XmlAttribute("team")]
        public TeamColor Team { get; set; }
    }
}
