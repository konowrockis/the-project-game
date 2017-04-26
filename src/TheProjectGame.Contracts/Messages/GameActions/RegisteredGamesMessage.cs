using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(ElementName = "RegisteredGames")]
    public class RegisteredGamesMessage : IMessage
    {
        [XmlElement("GameInfo")]
        public List<GameInfo> GameInfo { get; set; }
    }
}
