using System;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public abstract class Tile
    {
        public GamePlayer Player { get; set; }
        public DateTime Timestamp { get; set; }
        public uint X { get; }
        public uint Y { get; }

        public Tile(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public abstract Field ToField();
    }

    public class TaskTile : Tile
    {
        public BoardPiece Piece { get; set; }
        public uint DistanceToPiece { get; set; }

        public TaskTile(uint x, uint y) : base(x, y)
        { }

        public override Field ToField()
        {
            TaskField taskField = new TaskField()
            {
                PlayerIdSpecified = Player != null,
                DistanceToPiece = DistanceToPiece,
                PieceIdSpecified = Piece != null,
                Timestamp = DateTime.Now,
                X = X,
                Y = Y
            };

            if (taskField.PieceIdSpecified)
            {
                taskField.PieceId = Piece.Id;
            }
            if (taskField.PlayerIdSpecified)
            {
                taskField.PlayerId = Player.Id;
            }

            return taskField;
        }
    }

    public class GoalTile : Tile
    {
        public TeamColor Team { get; }
        public GoalFieldType Type { get; set; }

        public GoalTile(TeamColor team, uint x, uint y) : base(x, y)
        {
            Team = team;
        }

        public override Field ToField()
        {
            GoalField goalField = new GoalField()
            {
                PlayerIdSpecified = Player != null,
                Timestamp = DateTime.Now,
                X = X,
                Y = Y,
                Team = Team,
                Type = Type
            };
            if (goalField.PlayerIdSpecified)
            {
                goalField.PlayerId = Player.Id;
            }

            return goalField;
        }
    }
}
