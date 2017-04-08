using System;
using System.Collections.Generic;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class Board
    {
        public uint BoardWidth { get; }
        public uint BoardHeight { get; }
        public uint TaskAreaHeight { get; }
        public uint GoalAreaHeight { get; }
        public Tile[,] Fields { get; }

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
                    Fields[x, taskAreaHeight - y - 1] = new GoalTile(TeamColor.Blue, x, y);
                }

                for (uint y = goalAreaHeight; y < taskAreaHeight + goalAreaHeight; y++)
                {
                    Fields[x, y] = new TaskTile(x, y);
                }
            }
        }

        public void Init(IList<GamePlayer> players)
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
    }
}
