﻿using System.Xml.Serialization;
using CommandLine;

namespace TheProjectGame.Settings.Options
{
    [XmlRoot(ElementName = "PlayerSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class PlayerOptions : GeneralOptions
    {
        private const int DefaultRetryJoinGameInterval = 5000;

        [Option('g', "game")]
        public string NameOfTheGame { get; set; }

        [Option('t', "team")]
        public string TeamColor { get; set; }

        [Option('r', "role")]
        public string Role { get; set; }

        [XmlAttribute("RetryJoinGameInterval")]
        [Option(nameof(PlayerOptions) + "." + nameof(RetryJoinGameInterval))]
        public uint RetryJoinGameInterval { get; set; } = DefaultRetryJoinGameInterval;
    }
}
