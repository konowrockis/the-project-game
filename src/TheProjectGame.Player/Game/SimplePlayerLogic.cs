using System;
using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public class SimplePlayerLogic : IPlayerLogic
    {
        private Random random = new Random();
        private bool lastDiscovered = false;

        private readonly IPlayerKnowledge knowledge;

        public SimplePlayerLogic(IPlayerKnowledge knowledge)
        {
            this.knowledge = knowledge;
        }

        public IMessage GetNextMove()
        {
            var board = knowledge.GameState.Board;
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
                // has piece
                if (knowledge.CarriedPiece.Type == PieceType.Unknown)
                {
                    // piece unknown, test it
                    knowledge.ClearCarriedPiece();
                    TestPieceMessage testPiece = new TestPieceMessage()
                    {
                        PlayerGuid = knowledge.MyGuid,
                        GameId = knowledge.GameState.Id
                    };
                    return testPiece;
                }

                if (board.IsInGoalArea(playerPos))
                {
                    // is in goal area and on unknown goal tile, place piece
                    var tile = board.Fields[playerPos.X, playerPos.Y] as GoalTile;
                    if (tile.Type == GoalFieldType.Unknown && tile.Team == knowledge.Player.Team)
                    {
                        knowledge.ClearCarriedPiece();
                        PlacePieceMessage placePiece = new PlacePieceMessage()
                        {
                            PlayerGuid = knowledge.MyGuid,
                            GameId = knowledge.GameState.Id
                        };
                        return placePiece;
                    }
                }
                // not on undiscovered goal
                // go straight to first non-discovered goal
                return MoveToAnyNonDiscoveredGoal();
            }
            else
            {
                // doesnt have piece
                if (!board.IsInGoalArea(playerPos))
                {
                    // is in task area
                    var tile = board.Fields[playerPos.X, playerPos.Y] as TaskTile;
                    if (tile.Piece != null)
                    {
                        // task tile has piece, pick it up
                        PickUpPieceMessage pickup = new PickUpPieceMessage()
                        {
                            PlayerGuid = knowledge.MyGuid,
                            GameId = knowledge.GameState.Id
                        };
                        return pickup;
                    }
                    else
                    {
                        // task tile has no piece
                        if (lastDiscovered)
                        {
                            // if previously used discover go to tile with lowest distance to piece
                            lastDiscovered = false;
                            var tiles = board.GetNeighbourhood(playerPos.X, playerPos.Y).ToList().OfType<TaskTile>().OrderBy(t => t.DistanceToPiece, Comparer<int>.Default).ToList();
                            var dest = tiles[random.Next(tiles.Count())];
                            return MoveToward(new Position(dest.X, dest.Y));
                        }
                        else
                        {
                            // if discover not used previously use discover
                            DiscoverMessage discover = new DiscoverMessage()
                            {
                                PlayerGuid = knowledge.MyGuid,
                                GameId = knowledge.GameState.Id
                            };
                            lastDiscovered = true;
                            return discover;
                        }
                    }
                }
                else
                {
                    // is in goal area
                    // go to task area immediately!
                    MoveType direction = knowledge.Player.Team == TeamColor.Red ? MoveType.Down : MoveType.Up;
                    MoveMessage move = new MoveMessage()
                    {
                        Direction = CheckMove(direction),
                        PlayerGuid = knowledge.MyGuid,
                        GameId = knowledge.GameState.Id
                    };
                    return move;
                }
            }
        }

        private MoveType CheckMove(MoveType dir)
        {
            var position = knowledge.Player.Position.Move(dir);
            if (knowledge.GameState.Board.Fields[position.X, position.Y].Player != null)
            {
                return RandomMoveDirection();
            }
            return dir;
        }

        private MoveMessage MoveToward(Position destination)
        {
            Position current = knowledge.Player.Position;
            MoveType direction = MoveType.Down;

            if (current.X - destination.X != 0)
            {
                direction = current.X > destination.X ? MoveType.Left : MoveType.Right;
            }
            else
            {
                direction = current.Y > destination.Y ? MoveType.Up : MoveType.Down;
            }
            MoveMessage move = new MoveMessage()
            {
                GameId = knowledge.GameState.Id,
                Direction = CheckMove(direction),
                PlayerGuid = knowledge.MyGuid
            };
            return move;
        }

        private MoveMessage MoveToAnyNonDiscoveredGoal()
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

            direction = CheckMove(direction);

            MoveMessage move = new MoveMessage()
            {
                GameId = knowledge.GameState.Id,
                Direction = direction,
                PlayerGuid = knowledge.MyGuid
            };
            return move;

        }

        private MoveMessage RandomMove(PlayerKnowledge knowledge)
        {
            MoveMessage move = new MoveMessage()
            {
                GameId = knowledge.GameState.Id,
                Direction = RandomMoveDirection(),
                PlayerGuid = knowledge.MyGuid
            };
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

        public bool ShouldExchangeKnowledge()
        {
            if (knowledge.Player.Role == PlayerType.Leader)
            {
                return true;
            }

            return random.Next(100) < 70; // 70% chance to accept the request
        }
    }
}
