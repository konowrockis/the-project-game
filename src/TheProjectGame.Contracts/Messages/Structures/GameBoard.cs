using System;
using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class GameBoard
    {
        [XmlAttribute("width")]
        public uint Width { get; set; }

        [XmlAttribute("tasksHeight")]
        public uint TasksHeight { get; set; }

        [XmlAttribute("goalsHeight")]
        public uint GoalsHeight { get; set; }
    }
}
