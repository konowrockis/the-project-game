using System;
using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public class Board
    {
        public uint BoardWidth { get; }
        public uint BoardHeight { get; }
        public uint TaskAreaHeight { get; }
        public uint GoalAreaHeight { get; }
        public Tile[,] Fields { get; }
        public List<BoardPiece> Pieces { get; private set; }

        public Board(uint width, uint taskAreaHeight, uint goalAreaHeight)
        {
            BoardWidth = width;
            TaskAreaHeight = taskAreaHeight;
            GoalAreaHeight = goalAreaHeight;
            BoardHeight = taskAreaHeight + goalAreaHeight * 2;

            Fields = new Tile[BoardWidth, BoardHeight];

            for (uint x = 0; x < BoardWidth; x++)
            {
                for (uint y = 0; y < goalAreaHeight; y++)
                {
                    Fields[x, y] = new GoalTile(TeamColor.Red, x, y);
                    Fields[x, BoardHeight - y - 1] = new GoalTile(TeamColor.Blue, x, y);
                }

                for (uint y = goalAreaHeight; y < taskAreaHeight + goalAreaHeight; y++)
                {
                    Fields[x, y] = new TaskTile(x, y);
                }
            }
            Pieces = new List<BoardPiece>();
        }

        public void Init(IList<GamePlayer> players, uint pieceCount)
        {
            
        }

        public IEnumerable<Tile> GetNeighbourhood(int x, int y)
        {
            for (int currentX = Math.Max(0, x - 1); currentX < Math.Min(BoardWidth, x + 2); currentX++)
            {
                for (int currentY = Math.Max(0, y - 1); currentY < Math.Min(BoardHeight, y + 2); currentY++)
                {
                    yield return Fields[currentX, currentY];
                }
            }
        }

        public bool IsOccupied(Position position)
        {
            return IsOccupied(position.X, position.Y);
        }

        public bool IsOccupied(int x, int y)
        {
            return Fields[x,y].Player != null;
        }

        public bool IsValid(int x, int y)
        {
            return x>=0 & x < BoardWidth && y>=0 && y < BoardHeight;
        }

        public bool IsValid(Position position)
        {
            return IsValid(position.X, position.Y);
        }

        public Tuple<BoardPiece,int> FindClosestPiece(Position position)
        {
            return Pieces
                .Where(p=>p.Player==null)
                .Select(p=>new Tuple<BoardPiece,int>(p, position.ManhattanDistance(p.Position)))
                .OrderBy(p => p.Item2)
                .FirstOrDefault();
        }

        public void MovePlayer(GamePlayer player, Position destination)
        {
            // todo: move piece with player - what to do when on GoalFields
            Position pos = player.Position;
            Fields[pos.X,pos.Y].Player = null;
            player.Position = destination;
            Fields[destination.X, destination.Y].Player = player;
        }

    }
}
