﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public class SimplePlayerLogic : IPlayerLogic
    {
        private Random random = new Random();

        public IMessage GetNextMove(IBoard board, PlayerKnowledge knowledge)
        {
            // algorithm
            // 1. if on goal field and not carrying piece
            // 2. go in straight line to task fields
            // 3. after reaching every task field use discover
            // 4. then go to closest piece
            // 5. pick up piece
            // 6. check for sham
            // 7. if sham go back to 3.
            // 8. if not sham go to first non discovered goal starting from top left goal tiles (!!! deadlocks of characters possible)
            // 9. discover goal and go back to 1.
            var playerPos = knowledge.Player.Position;

            if (knowledge.CarriedPiece != null)
            {
                if (board.IsInGoalArea(playerPos))
                {
                    var tile = board.Fields[playerPos.X, playerPos.Y] as GoalTile;
                    if (tile.Type == GoalFieldType.Unknown && tile.Team == knowledge.Player.Team)
                    {
                        knowledge.CarriedPiece = null;
                        PlacePiece placePiece = new PlacePiece();
                        placePiece.PlayerGuid = knowledge.Guid;
                        placePiece.GameId = knowledge.GameState.Id;
                        return placePiece;
                    }
                }
                // go straight to first non-discovered goal
                return MoveToAnyNonDiscoveredGoal(knowledge);
            }
            else
            {
                if (!board.IsInGoalArea(playerPos))
                {
                    var tile = board.Fields[playerPos.X, playerPos.Y] as TaskTile;
                    if (tile.Piece != null)
                    {
                        PickUpPiece pickup = new PickUpPiece();
                        pickup.PlayerGuid = knowledge.Guid;
                        pickup.GameId = knowledge.GameState.Id;
                        return pickup;
                    }
                }
                /*if (random.NextDouble() > 0.8 && !board.IsInGoalArea(playerPos))
                {
                    Discover discover = new Discover();
                    discover.PlayerGuid = knowledge.Guid;
                    discover.GameId = knowledge.GameState.Id;
                    return discover;
                }*/
                return RandomMove(knowledge);
            }
        }

        private Move MoveToAnyNonDiscoveredGoal(PlayerKnowledge knowledge)
        {
            var playerPos = knowledge.Player.Position;
            var board = knowledge.GameState.Board;
            var goalTiles = board.GetGoalTiles(knowledge.Player.Team);
            var unknownTiles = goalTiles.Where(g => g.Type == GoalFieldType.Unknown).ToList();
            var selected = unknownTiles.First();

            var x = selected.X;
            var y = selected.Y;

            MoveType direction = MoveType.Down;

            if (playerPos.X - x != 0)
            {
                direction = playerPos.X > x ? MoveType.Left : MoveType.Right;
            }
            else
            {
                direction = playerPos.Y > y ? MoveType.Up : MoveType.Down;
            }

            Move move = new Move();
            move.GameId = knowledge.GameState.Id;
            move.Direction = direction;
            move.PlayerGuid = knowledge.Guid;
            return move;

        }

        private Move RandomMove(PlayerKnowledge knowledge)
        {
            Move move = new Move();
            move.GameId = knowledge.GameState.Id;
            move.Direction = RandomMoveDirection();
            move.PlayerGuid = knowledge.Guid;
            return move;
        }

        private MoveType RandomMoveDirection()
        {
            switch (random.Next(4))
            {
                case 0:
                    return MoveType.Down;
                case 1:
                    return MoveType.Up;
                case 2:
                    return MoveType.Left;
                case 3:
                    return MoveType.Right;
                default:
                    return MoveType.Down;
            }
        }

    }
}
