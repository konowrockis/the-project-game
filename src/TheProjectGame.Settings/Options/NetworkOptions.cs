using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class NetworkOptions
    {
        [Option('p', "port")]
        public int Port { get; set; }

        [Option('a', "address")]
        public string Address { get; set; }

        [Option(nameof(NetworkOptions) + "." + nameof(KeepAliveInterval))]
        public int KeepAliveInterval { get; set; }
    }
}
