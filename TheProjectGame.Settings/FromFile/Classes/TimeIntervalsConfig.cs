using System.Runtime.Serialization;

namespace TheProjectGame.Settings.FromFile.Classes
{
    [DataContract]
    public class TimeIntervalsConfig
    {
        [DataMember]
        public uint MoveDelay { get; private set; }
        [DataMember]
        public uint DiscoveryDelay { get; private set; }
        [DataMember]
        public uint TestDelay { get; private set; }
        [DataMember]
        public uint PickupDelay { get; private set; }
        [DataMember]
        public uint PlacingDelay { get; private set; }
        [DataMember]
        public uint KnowledgeExchange { get; private set; }
    }
}
