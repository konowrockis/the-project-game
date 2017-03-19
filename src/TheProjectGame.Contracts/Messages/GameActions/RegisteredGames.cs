using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class RegisteredGames : IMessage
    {
        [XmlElement("GameInfo")]
        public List<GameInfo> GameInfo { get; set; }
    }
}
