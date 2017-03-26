using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CommandLine;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    class OptionsParser
    {
        private const string defaultConfigLocation = "config.xml";

        private readonly Parser parser;
        private readonly string configLocation;
        private readonly string[] args;

        public OptionsParser(string[] args)
        {
            this.args = args;

            parser = new Parser();
            configLocation = getConfigLocation();
        }

        public T GetOptions<T>() where T : NetworkOptions, new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T value = new T();

            if (File.Exists(configLocation) && configLocation.EndsWith(".xml"))
            {
                value = serializer.Deserialize(File.OpenRead(configLocation)) as T;
            }
            else
            {
                Console.WriteLine("No suitable configuration file found.");
            }

            parser.ParseArguments(args, value);

            return value;
        }

        private string getConfigLocation()
        {
            ConfigLocation config = new ConfigLocation();
            parser.ParseArguments(args, config);

            return string.IsNullOrWhiteSpace(config.ConfigurationPath) ?
                defaultConfigLocation :
                config.ConfigurationPath;
        }
    }
}
