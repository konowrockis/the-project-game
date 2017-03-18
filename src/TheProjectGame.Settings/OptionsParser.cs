using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CommandLine;

namespace TheProjectGame.Settings
{
    class OptionsParser
    {
        private const string defaultConfigLocation = "config.xml";

        private readonly Parser parser;
        private readonly string configLocation;
        private readonly XmlNode configNode;
        private readonly string[] args;

        public OptionsParser(string[] args)
        {
            this.args = args;

            parser = new Parser();
            configLocation = getConfigLocation();
            configNode = getConfigNode();
        }

        public IOptions<T> GetOptions<T>() where T : class, new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var node = configNode?.ChildNodes?.Cast<XmlNode>()
                .FirstOrDefault(n => n.Name == typeof(T).Name);

            T value = new T();

            if (node != null)
            {
                using (XmlNodeReader reader = new XmlNodeReader(node))
                {
                    value = serializer.Deserialize(reader) as T;
                }
            }

            parser.ParseArguments(args, value);

            return new Options<T>(value);
        }

        private string getConfigLocation()
        {
            Options.ConfigLocation config = new Options.ConfigLocation();
            parser.ParseArguments(args, config);

            return string.IsNullOrWhiteSpace(config.ConfigurationPath) ?
                defaultConfigLocation :
                config.ConfigurationPath;
        }

        private XmlNode getConfigNode()
        {
            if (!File.Exists(configLocation) || !configLocation.EndsWith(".xml"))
            {
                Console.WriteLine("No suitable configuration file found.");
                return null;
            }

            XmlDocument document = new XmlDocument();
            document.Load(configLocation);

            return document.ChildNodes.Cast<XmlNode>()
                .FirstOrDefault(n => n.Name == "settings");
        }

        private class Options<T> : IOptions<T> where T : class
        {
            public T Value { get; }

            public Options(T value)
            {
                Value = value;
            }
        }
    }
}
