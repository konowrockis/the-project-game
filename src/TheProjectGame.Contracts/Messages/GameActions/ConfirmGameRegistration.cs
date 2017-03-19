﻿using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.GameActions
{
    [XmlRoot(Namespace = "http://theprojectgame.mini.pw.edu.pl/")]
    public class ConfirmGameRegistration : IMessage
    {
        [XmlAttribute("gameId")]
        public ulong GameId { get; set; }
    }
}
