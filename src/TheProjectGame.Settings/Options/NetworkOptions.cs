using System.Xml.Serialization;
using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class NetworkOptions
    {
        private const int DefaultKeepAliveInterval = 30000;

        [Option('p', "port")]
        public int Port { get; set; }

        [Option('a', "address")]
        public string Address { get; set; }

        [XmlAttribute("KeepAliveInterval")]
        [Option(nameof(NetworkOptions) + "." + nameof(KeepAliveInterval))]
        public int KeepAliveInterval { get; set; } = DefaultKeepAliveInterval;
    }
}
