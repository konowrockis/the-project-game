using System.Runtime.Serialization;

namespace TheProjectGame.Settings.FromFile.Classes
{
    [DataContract]
    public class TechnicalConfig
    {
        [DataMember]
        public uint KeepAliveInterval { get; private set; }
    }
}
