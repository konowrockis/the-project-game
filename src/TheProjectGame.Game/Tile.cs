using System;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class Tile
    {
        public GamePlayer Player { get; private set; }
        public DateTime Timestamp { get; private set; }
        public uint X { get; }
        public uint Y { get; }

        public Tile(uint x, uint y)
        {
            X = x;
            Y = y;
        }
    }

    public class TaskTile : Tile
    {
        public BoardPiece Piece { get; private set; }
        public uint DistanceToPiece { get; private set; }

        public TaskTile(uint x, uint y) : base(x, y)
        { }
    }

    public class GoalTile : Tile
    {
        public TeamColor Team { get; }
        public GoalFieldType Type { get; private set; }

        public GoalTile(TeamColor team, uint x, uint y) : base(x, y)
        {
            Team = team;
        }
    }
}
