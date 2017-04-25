using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game.Builders
{
    public class ObjectMapper
    {
        public static Location Map(Position position)
        {
            return new Location((uint)position.X, (uint)position.Y);
        }

        public static Field Map(Tile tile)
        {
            if (tile is TaskTile) return Map(tile as TaskTile);
            return Map(tile as GoalTile);
        }

        public static TaskField Map(TaskTile taskTile)
        {
            TaskField taskField = new TaskField()
            {
                PlayerIdSpecified = taskTile.Player != null,
                DistanceToPiece = taskTile.DistanceToPiece,
                PieceIdSpecified = taskTile.Piece != null,
                Timestamp = Time.Now,
                X = taskTile.X,
                Y = taskTile.Y
            };

            if (taskField.PieceIdSpecified)
            {
                taskField.PieceId = taskTile.Piece.Id;
            }
            if (taskField.PlayerIdSpecified)
            {
                taskField.PlayerId = taskTile.Player.Id;
            }

            return taskField;
        }

        public static GoalField Map(GoalTile goalTile)
        {
            GoalField goalField = new GoalField()
            {
                PlayerIdSpecified = goalTile.Player != null,
                Timestamp = Time.Now,
                X = goalTile.X,
                Y = goalTile.Y,
                Team = goalTile.Team,
                Type = goalTile.Type
            };
            if (goalField.PlayerIdSpecified)
            {
                goalField.PlayerId = goalTile.Player.Id;
            }

            return goalField;
        }

        public static Piece Map(BoardPiece boardPiece, bool discovered=false)
        {
            var piece = new Piece()
            {
                Id = boardPiece.Id,
                Timestamp = Time.Now,
                Type = PieceType.Unknown,
            };
            if (discovered)
            {
                piece.Type = boardPiece.Type;
            }
            if (boardPiece.Player != null)
            {
                piece.PlayerIdSpecified = true;
                piece.PlayerId = boardPiece.Player.Id;
            }
            return piece;
        }

        public static Player Map(GamePlayer gamePlayer)
        {
            Player player = new Player()
            {
                Id = gamePlayer.Id,
                Team = gamePlayer.Team,
                Type = gamePlayer.Role
            };
            return player;
        }
    }
}
