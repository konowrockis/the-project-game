using System.Collections.Generic;
using System.Xml.Serialization;
using CommandLine;
using TheProjectGame.Settings.Options.Structures;

namespace TheProjectGame.Settings.Options
{
    public class GameOptions
    {
        private const double DefaultShamProbability = 0.1;
        private const uint DefaultPlacingNewPiecesFrequency = 1000;
        private const uint DefaultInitialNumberOfPieces = 4;
        private const uint DefaultBoardWidth = 5;
        private const uint DefaultTaskAreaLength = 7;
        private const uint DefaultGoalAreaLength = 3;
        private const uint DefaultNumberOfPlayersPerTeam = 4;

        [XmlElement(ElementName = "Goals")]
        public List<GoalField> Goals { get; set; } = new List<GoalField>();

        [Option(nameof(GameOptions) + "." + nameof(ShamProbability))]
        public double ShamProbability { get; set; } = DefaultShamProbability;

        [Option(nameof(GameOptions) + "." + nameof(PlacingNewPiecesFrequency))]
        public uint PlacingNewPiecesFrequency { get; set; } = DefaultPlacingNewPiecesFrequency;

        [Option(nameof(GameOptions) + "." + nameof(InitialNumberOfPieces))]
        public uint InitialNumberOfPieces { get; set; } = DefaultInitialNumberOfPieces;

        [Option(nameof(GameOptions) + "." + nameof(BoardWidth))]
        public uint BoardWidth { get; set; } = DefaultBoardWidth;

        [Option(nameof(GameOptions) + "." + nameof(TaskAreaLength))]
        public uint TaskAreaLength { get; set; } = DefaultTaskAreaLength;

        [Option(nameof(GameOptions) + "." + nameof(GoalAreaLength))]
        public uint GoalAreaLength { get; set; } = DefaultGoalAreaLength;

        [Option(nameof(GameOptions) + "." + nameof(NumberOfPlayersPerTeam))]
        public uint NumberOfPlayersPerTeam { get; set; } = DefaultNumberOfPlayersPerTeam;

        [Option(nameof(GameOptions) + "." + nameof(GameName))]
        public string GameName { get; set; }
    }
}
