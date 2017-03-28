using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CommandLine;
using Serilog;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    class OptionsParser
    {
        private readonly ILogger logger = Log.ForContext<OptionsParser>();

        private readonly string[] args;
        private readonly IOptionsSource optionsSource;

        public OptionsParser(string[] args, IOptionsSource optionsSource)
        {
            this.args = args;
            this.optionsSource = optionsSource;
        }

        public T GetOptions<T>() where T : GeneralOptions, new()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T value = new T();
            var configurationSource = optionsSource.GetConfiguration();

            if (configurationSource != null)
            {
                value = serializer.Deserialize(configurationSource) as T;

                configurationSource.Dispose();
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

        
    }
}
