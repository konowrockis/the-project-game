using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game.Builders
{
    public class DataBuilder
    {
        private readonly Data data;

        public DataBuilder()
        {
            data = new Data();
        }

        public DataBuilder PlayerId(ulong id)
        {
            data.PlayerId = id;
            return this;
        }

        public DataBuilder GameFinished(bool val)
        {
            data.GameFinished = val;
            return this;
        }

        public DataBuilder PlayerLocation(Location val)
        {
            data.PlayerLocation = val;
            return this;
        }

        public DataBuilder Fields(Board board, params Tile[] tiles)
        {
            var tileList = tiles.ToList();

            var taskTiles = tileList.OfType<TaskTile>().ToList();
            var gameTiles = tileList.OfType<GoalTile>().ToList();

            var goalFields = gameTiles.Select(tile => tile.ToField() as GoalField).ToList();
            var taskFields =
                taskTiles.Select(
                    tile =>
                        new Tuple<TaskField, Tuple<BoardPiece, int>>(tile.ToField() as TaskField,
                            board.FindClosestPiece(new Position(tile.X, tile.Y)))).ToList();

            var pieces = new List<Piece>();

            taskFields.ForEach(fieldData =>
            {
                fieldData.Item1.DistanceToPiece = fieldData.Item2 == null? uint.MaxValue : (uint)fieldData.Item2.Item2;
                if (fieldData.Item1.DistanceToPiece == 0)
                {
                    pieces.Add(fieldData.Item2.Item1.ToPiece());
                }
            });

            data.GoalFields = goalFields;
            data.TaskFields = taskFields.Select(fieldData => fieldData.Item1).ToList();
            data.Pieces = pieces;
            return this;
        }

        public Data Build()
        {
            return data;
        }
    }
}
