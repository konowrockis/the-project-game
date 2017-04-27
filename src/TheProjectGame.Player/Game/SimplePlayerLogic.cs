using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public class SimplePlayerLogic : IPlayerLogic
    {
        private readonly ILogger logger = Log.ForContext<SimplePlayerLogic>();
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
            var player = knowledge.Player;
            var position = player.Position;
            var piece = knowledge.CarriedPiece;
            var tile = board.Fields[position.X, position.Y];
            var taskTile = tile as TaskTile;
            var goalTile = tile as GoalTile;

            var playerState = ResolvePlayerState();

            switch (playerState.PieceState)
            {
                case PlayerPieceState.HasUnknownPiece:
                    return ActionTestPiece();
                case PlayerPieceState.HasNormalPiece:
                    if (playerState.PositionState == PlayerPositionState.IsOnOwnGoalField &&
                        goalTile.Type == GoalFieldType.Unknown)
                    {
                        return ActionPlacePiece();
                    }
                    else
                    {
                        return ActionMove(DirectionToAnyNonDiscoveredGoal());
                    }
                case PlayerPieceState.HasNoPiece:
                    switch (playerState.PositionState)
                    {
                        case PlayerPositionState.IsOnEnemyGoalField:
                            return ActionMove(player.Team == TeamColor.Blue ? MoveType.Down : MoveType.Up);
                        case PlayerPositionState.IsOnOwnGoalField:
                            return ActionMove(player.Team == TeamColor.Blue ? MoveType.Up : MoveType.Down);
                        case PlayerPositionState.IsOnTaskField:
                            if (taskTile.Piece != null)
                            {
                                return ActionPickUpPiece();
                            }
                            else if (lastDiscovered)
                            {
                                lastDiscovered = false;
                                var tiles =
                                    board.GetNeighbourhood(position.X, position.Y)
                                        .ToList()
                                        .OfType<TaskTile>()
                                        .OrderBy(t => t.DistanceToPiece, Comparer<int>.Default)
                                        .ToList();
                                var dest = tiles[random.Next(tiles.Count())];
                                return ActionMove(DirectionTowards(new Position(dest.X, dest.Y)));
                            }
                            else
                            {
                                return ActionDiscover();
                            }
                    }
                    break;
            }
            logger.Error("Invalid player state");
            return null;
        }

        private IMessage ActionTestPiece()
        {
            knowledge.ClearCarriedPiece();
            TestPieceMessage testPiece = new TestPieceMessage()
            {
                PlayerGuid = knowledge.MyGuid,
                GameId = knowledge.GameState.Id
            };
            logger.Debug("Testing piece {@Message}", testPiece);
            return testPiece;
        }

        private IMessage ActionPlacePiece()
        {
            knowledge.ClearCarriedPiece();
            PlacePieceMessage placePiece = new PlacePieceMessage()
            {
                PlayerGuid = knowledge.MyGuid,
                GameId = knowledge.GameState.Id
            };
            logger.Debug("Placing piece {@Message}", placePiece);

            return placePiece;
        }

        private IMessage ActionMove(MoveType direction)
        {
            MoveMessage move = new MoveMessage()
            {
                GameId = knowledge.GameState.Id,
                Direction = CheckMoveDirection(direction),
                PlayerGuid = knowledge.MyGuid
            };
            logger.Debug("Moving {@Message}", move);
            return move;
        }

        private IMessage ActionPickUpPiece()
        {
            PickUpPieceMessage pickup = new PickUpPieceMessage()
            {
                PlayerGuid = knowledge.MyGuid,
                GameId = knowledge.GameState.Id
            };

            logger.Debug("Picking up piece {@Message}", pickup);
            return pickup;
        }

        private IMessage ActionDiscover()
        {
            DiscoverMessage discover = new DiscoverMessage()
            {
                PlayerGuid = knowledge.MyGuid,
                GameId = knowledge.GameState.Id
            };
            lastDiscovered = true;
            logger.Debug("Discovering... {@Message}", discover);
            return discover;
        }

        private MoveType CheckMoveDirection(MoveType dir)
        {
            var position = knowledge.Player.Position.Move(dir);
            if (knowledge.GameState.Board.IsValid(position) &&
                knowledge.GameState.Board.Fields[position.X, position.Y].Player != null)
            {
                return RandomMoveDirection();
            }
            return dir;
        }

        private MoveType DirectionTowards(Position destination)
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
            return direction;
        }

        private MoveType DirectionToAnyNonDiscoveredGoal()
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

            return direction;
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


        private PlayerState ResolvePlayerState()
        {
            var board = knowledge.GameState.Board;
            var player = knowledge.Player;
            var position = player.Position;
            var piece = knowledge.CarriedPiece;

            PlayerPieceState pieceState = PlayerPieceState.HasNoPiece;
            PlayerPositionState positionState = PlayerPositionState.IsOnTaskField;

            if (piece != null)
            {
                pieceState = piece.Type != PieceType.Unknown
                    ? PlayerPieceState.HasNormalPiece
                    : PlayerPieceState.HasUnknownPiece;
            }

            if (board.IsInGoalArea(position))
            {
                var otherTeam = knowledge.Player.Team == TeamColor.Blue ? TeamColor.Red : TeamColor.Blue;
                var otherTeamGoalTilePositions =
                    board.GetGoalTiles(otherTeam).Select(t => new Position(t.X, t.Y)).ToList();

                if (otherTeamGoalTilePositions.Find(p => p.X == position.X && p.Y == position.Y) != null)
                {
                    positionState = PlayerPositionState.IsOnEnemyGoalField;
                }
                else
                {
                    positionState = PlayerPositionState.IsOnOwnGoalField;
                }
            }

            return new PlayerState(pieceState, positionState);
        }
    }

    enum PlayerPositionState
    {
        IsOnTaskField,
        IsOnOwnGoalField,
        IsOnEnemyGoalField
    }

    enum PlayerPieceState
    {
        HasNoPiece,
        HasUnknownPiece,
        HasNormalPiece
    }

    class PlayerState
    {
        public PlayerPieceState PieceState { get; }
        public PlayerPositionState PositionState { get; }

        public PlayerState(PlayerPieceState pieceState, PlayerPositionState positionState)
        {
            PieceState = pieceState;
            PositionState = positionState;
        }
    }
}
