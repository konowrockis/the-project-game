using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class ActionCostsOptions
    {
        private const uint DefaultMoveDelay = 100;
        private const uint DefaultDiscoverDelay = 450;
        private const uint DefaultTestDelay = 500;
        private const uint DefaultPickUpDelay = 100;
        private const uint DefaultPlacingDelay = 100;
        private const uint DefaultKnowledgeExchangeDelay = 1200;

        [Option(nameof(ActionCostsOptions) + "." + nameof(MoveDelay))]
        public uint MoveDelay { get; set; } = DefaultMoveDelay;

        [Option(nameof(ActionCostsOptions) + "." + nameof(DiscoverDelay))]
        public uint DiscoverDelay { get; set; } = DefaultDiscoverDelay;

        [Option(nameof(ActionCostsOptions) + "." + nameof(TestDelay))]
        public uint TestDelay { get; set; } = DefaultTestDelay;

        [Option(nameof(ActionCostsOptions) + "." + nameof(PickUpDelay))]
        public uint PickUpDelay { get; set; } = DefaultPickUpDelay;

        [Option(nameof(ActionCostsOptions) + "." + nameof(PlacingDelay))]
        public uint PlacingDelay { get; set; } = DefaultPlacingDelay;

        [Option(nameof(ActionCostsOptions) + "." + nameof(KnowledgeExchangeDelay))]
        public uint KnowledgeExchangeDelay { get; set; } = DefaultKnowledgeExchangeDelay;
        
    }
}
