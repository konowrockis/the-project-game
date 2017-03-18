using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TheProjectGame.Settings.FromCommandLine
{
    public class PlayerParameters : GameMasterParameters
    {
        [Option('g', "game")]
        public string NameOfTheGame { get; set; }

        [Option('t', "team")]
        public string TeamColor { get; set; }

        [Option('r', "role")]
        public string Role { get; set; }

        public PlayerParameters(string[] args) : base(args)
        {
        }
    }
}
