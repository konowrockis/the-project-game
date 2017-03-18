using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class DelaysOptions
    {
        [Option(nameof(DelaysOptions) + "." + nameof(MoveDelay))]
        public uint MoveDelay { get; set; }

        [Option(nameof(DelaysOptions) + "." + nameof(DiscoveryDelay))]
        public uint DiscoveryDelay { get; set; }

        [Option(nameof(DelaysOptions) + "." + nameof(TestDelay))]
        public uint TestDelay { get; set; }

        [Option(nameof(DelaysOptions) + "." + nameof(PickupDelay))]
        public uint PickupDelay { get; set; }

        [Option(nameof(DelaysOptions) + "." + nameof(PlacingDelay))]
        public uint PlacingDelay { get; set; }

        [Option(nameof(DelaysOptions) + "." + nameof(KnowledgeExchangeDelay))]
        public uint KnowledgeExchangeDelay { get; set; }
    }
}
