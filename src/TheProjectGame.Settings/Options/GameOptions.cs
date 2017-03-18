using CommandLine;

namespace TheProjectGame.Settings.Options
{
    public class GameOptions
    {
        [Option(nameof(GameOptions) + "." + nameof(ProbabilityOfSham))]
        public double ProbabilityOfSham { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(GoalDefinition))]
        public double FrequencyOfPlacingPieces { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(NumberOfPieces))]
        public int NumberOfPieces { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(BoardWidth))]
        public int BoardWidth { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(TaskAreaLength))]
        public int TaskAreaLength { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(SingleGoalsAreaLength))]
        public int SingleGoalsAreaLength { get; set; }

        [Option(nameof(GameOptions) + "." + nameof(NumberOfPlayers))]
        public int NumberOfPlayers { get; set; }

        [Option(nameof(GameOptions)+ "." + nameof(GoalDefinition))]
        public string GoalDefinition { get; set; }
    }
}
