using System.IO;
using CommandLine;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    class OptionsSource : IOptionsSource
    {
        private const string defaultConfigLocation = "config.xml";

        private readonly string[] args;

        public OptionsSource(string[] args)
        {
            this.args = args;
        }

        public Stream GetConfiguration()
        {
            var configLocation = getConfigLocation(args);

            if (File.Exists(configLocation) && configLocation.EndsWith(".xml"))
            {
                return File.OpenRead(configLocation);
            }
            else return null;
        }

        private string getConfigLocation(string[] args)
        {
            ConfigLocation config = new ConfigLocation();
            Parser.Default.ParseArguments(args, config);

            return string.IsNullOrWhiteSpace(config.ConfigurationPath) ?
                defaultConfigLocation :
                config.ConfigurationPath;
        }
    }
}
