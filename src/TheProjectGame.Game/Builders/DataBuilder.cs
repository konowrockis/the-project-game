﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;
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

        public DataBuilder Fields(params Tile[] tiles)
        {
            var tileList = tiles.ToList();

            var taskTiles = tileList.OfType<TaskTile>().ToList();
            var gameTiles = tileList.OfType<GoalTile>().ToList();

            var goalFields = gameTiles.Where(tile=>tile.Discovered).Select(ObjectMapper.Map).ToList();
            var nonDiscoveredGoalFields = gameTiles.Where(tile => !tile.Discovered).Select(ObjectMapper.Map).ToList();
            nonDiscoveredGoalFields.ForEach(field => field.Type = GoalFieldType.Unknown);
            goalFields.AddRange(nonDiscoveredGoalFields);
            
            var taskFields = taskTiles.Select(ObjectMapper.Map).ToList();

            var pieces =
                taskTiles.Where(tile => tile.Piece != null).Select(tile => tile.Piece).Select(ObjectMapper.Map).ToList();

            data.GoalFields = goalFields;
            data.TaskFields = taskFields;
            data.Pieces = pieces;
            return this;
        }

        public Data Build()
        {
            return data;
        }
    }
}