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
        
    }

    public class TaskTile : Tile
    {
        public BoardPiece Piece { get; set; }
        public int DistanceToPiece { get; set; }

        public TaskTile(uint x, uint y) : base(x, y)
        { }
        
    }

    public class GoalTile : Tile
    {
        public TeamColor Team { get; }
        public GoalFieldType Type { get; set; }

        public GoalTile(TeamColor team, uint x, uint y) : base(x, y)
        {
            Team = team;
        }

    }
}
