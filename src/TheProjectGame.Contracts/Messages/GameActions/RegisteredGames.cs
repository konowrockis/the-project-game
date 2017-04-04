using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class RegisteredGames : IMessage
    {
        [XmlArray("GameInfo")]
        public List<GameInfo> GameInfo { get; set; }
    }
}
