using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class ConfigLocation
    {
        [Option('c', "configuration")]
        public string ConfigurationPath { get; set; }
    }
}
