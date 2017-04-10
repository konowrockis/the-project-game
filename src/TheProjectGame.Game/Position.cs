using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position(uint x, uint y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public int ManhattanDistance(Position position)
        {
            return Math.Abs(X - position.X) + Math.Abs(Y - position.Y);
        }

        public Location ToLocation()
        {
            return new Location((uint)X,(uint)Y);
        }

        public Position Move(MoveType direction)
        {
            Position destination = null;

            switch (direction)
            {
                case MoveType.Down:
                    destination = new Position(X, Y + 1);
                    break;
                case MoveType.Left:
                    destination = new Position(X - 1, Y);
                    break;
                case MoveType.Right:
                    destination = new Position(X + 1, Y);
                    break;
                case MoveType.Up:
                    destination = new Position(X, Y - 1);
                    break;
            }
            return destination;
        }

    }
}
