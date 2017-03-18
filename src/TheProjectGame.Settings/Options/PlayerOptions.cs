using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class PlayerOptions
    {
        [Option('g', "game")]
        public string NameOfTheGame { get; set; }

        [Option('t', "team")]
        public string TeamColor { get; set; }

        [Option('r', "role")]
        public string Role { get; set; }
    }
}
