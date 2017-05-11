using System.Xml.Serialization;
using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class GeneralOptions
    {
        private const int DefaultKeepAliveInterval = 30000;

        [Option('p', "port")]
        public int Port { get; set; }

        [Option('a', "address")]
        public string Address { get; set; }

        [XmlAttribute("KeepAliveInterval")]
        [Option(nameof(GeneralOptions) + "." + nameof(KeepAliveInterval))]
        public int KeepAliveInterval { get; set; } = DefaultKeepAliveInterval;

        [Option('v', "verbose")]
        public bool Verbose { get; set; }

        [Option('d', "display")]
        public bool Display { get; set; }

    }
}
