using System.Runtime.Serialization;

namespace TheProjectGame.Settings.FromFile.Classes
{
    [DataContract]
    public class GameConfig
    {
        [DataMember]
        public double ProbabilityOfSham { get; private set; }
        [DataMember]
        public double FrequencyOfPlacingPieces { get; private set; }
        [DataMember]
        public uint NumberOfPieces { get; private set; }
        [DataMember]
        public uint BoardWidth { get; private set; }
        [DataMember]
        public uint TaskAreaLength { get; private set; }
        [DataMember]
        public uint SingleGoalsAreaLength { get; private set; }
        [DataMember]
        public short NumberOfPlayers { get; private set; }
        [DataMember]
        public string GoalDefinition { get; private set; }
    }
}
