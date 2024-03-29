﻿using System;
using TheProjectGame.Contracts.Enums;

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

        public Position Move(MoveType direction)
        {
            Position destination = null;

            switch (direction)
            {
                case MoveType.Down:
                    destination = new Position(X, Y - 1);
                    break;
                case MoveType.Left:
                    destination = new Position(X - 1, Y);
                    break;
                case MoveType.Right:
                    destination = new Position(X + 1, Y);
                    break;
                case MoveType.Up:
                    destination = new Position(X, Y + 1);
                    break;
            }
            return destination;
        }

    }
}
