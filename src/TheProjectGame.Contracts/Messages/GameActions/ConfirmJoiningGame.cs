﻿using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    public class ConfirmJoiningGame : PlayerMessage, IMessage
    {
        public Player PlayerDefinition { get; set; }

        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }

        [XmlAttribute("privateGuid")]
        public string PrivateGuid { get; set; }
    }
}
