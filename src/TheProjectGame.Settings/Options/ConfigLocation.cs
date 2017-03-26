using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class ConfigLocation
    {
        [Option('c', "conf")]
        public string ConfigurationPath { get; set; }
    }
}
