using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game.Builders
{
    public class DataBuilder
    {
        private readonly IMapper mapper;
        private readonly DataMessage data;

        public DataBuilder(IMapper mapper)
        {
            this.mapper = mapper;

            data = new DataMessage();
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

        public DataBuilder PlayerLocation(Location location)
        {
            data.PlayerLocation = location;
            return this;
        }

        public DataBuilder PlayerLocation(Position position)
        {
            return PlayerLocation(mapper.Map<Location>(position));
        }

        public DataBuilder Fields(params Tile[] tiles)
        {
            var tileList = tiles.ToList();

            var taskTiles = tileList.OfType<TaskTile>().ToList();
            var gameTiles = tileList.OfType<GoalTile>().ToList();

            var goalFields = gameTiles
                .Where(tile => tile.Discovered)
                .Select(mapper.Map<GoalField>).ToList();

            var nonDiscoveredGoalFields = gameTiles
                .Where(tile => !tile.Discovered)
                .Select(mapper.Map<GoalField>).ToList();

            var pieces = taskTiles
                .Where(tile => tile.Piece != null)
                .Select(tile => tile.Piece)
                .Select(mapper.Map<Piece>).ToList();

            pieces.ForEach(p => p.Type = PieceType.Unknown);
            nonDiscoveredGoalFields.ForEach(field => field.Type = GoalFieldType.Unknown);
            goalFields.AddRange(nonDiscoveredGoalFields);

            var taskFields = taskTiles.Select(mapper.Map<TaskField>).ToList();

            data.GoalFields = goalFields;
            data.TaskFields = taskFields;
            if (data.Pieces != null)
            {
                data.Pieces.AddRange(pieces);
            }
            else data.Pieces = pieces;
            return this;
        }

        public DataBuilder Pieces(bool discovered, params BoardPiece[] pieces)
        {
            if (data.Pieces != null)
            {
                data.Pieces.AddRange(pieces.ToList().Select(mapper.Map<Piece>).ToList());
            } 
            else
            { 
                data.Pieces = pieces.ToList().Select(mapper.Map<Piece>).ToList();
            }

            if (!discovered)
            {
                data.Pieces.ForEach(p => p.Type = PieceType.Unknown);
            }
            return this;
        }

        public DataMessage Build()
        {
            return data;
        }
    }
}