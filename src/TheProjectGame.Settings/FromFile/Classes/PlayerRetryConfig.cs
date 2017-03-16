using System.Runtime.Serialization;

namespace TheProjectGame.Settings.FromFile.Classes
{
    [DataContract]
    public class PlayerRetryConfig
    {
        [DataMember]
        public uint RetryInterval { get; private set; }
    }
}
