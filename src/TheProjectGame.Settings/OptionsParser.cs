using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CommandLine;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    public class OptionsParser
    {
        private const string defaultConfigLocation = "config.xml";

        private readonly string configLocation;
        private readonly string[] args;

        public OptionsParser(string[] args)
        {
            this.args = args;

            configLocation = getConfigLocation();
        }

        public T GetOptions<T>() where T : GeneralOptions, new()
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

            ParseRecursive(args, value);

            return value;
        }

        private void ParseRecursive(string[] args, object value)
        {
            Parser.Default.ParseArguments(args, value);

            foreach (var property in value.GetType().GetProperties().Where(p => p.PropertyType.IsClass))
            {
                try
                {
                    var propertyValue = property.GetValue(value);

                    ParseRecursive(args, propertyValue);

                    property.SetValue(value, propertyValue);
                }
                catch { }
            }
        }

        private string getConfigLocation()
        {
            ConfigLocation config = new ConfigLocation();
            Parser.Default.ParseArguments(args, config);

            return string.IsNullOrWhiteSpace(config.ConfigurationPath) ?
                defaultConfigLocation :
                config.ConfigurationPath;
        }
    }
}
