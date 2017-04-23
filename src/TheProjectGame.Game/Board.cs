using System;
using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public class Board : IBoard
    {
        public uint BoardWidth { get; }
        public uint BoardHeight { get; }
        public uint TaskAreaHeight { get; }
        public uint GoalAreaHeight { get; }
        public Tile[,] Fields { get; }
        public List<BoardPiece> Pieces { get; private set; }

        private uint _pieceId = 1;
        private double shamProbability;

        private uint NextPieceId
        {
            get
            {
                uint val = _pieceId;
                _pieceId++;
                return val;
            }
        }

        public Board()
        {
            
        }

        public Board(uint width, uint taskAreaHeight, uint goalAreaHeight, double shamProbability)
        {
            BoardWidth = width;
            TaskAreaHeight = taskAreaHeight;
            GoalAreaHeight = goalAreaHeight;
            BoardHeight = taskAreaHeight + goalAreaHeight*2;
            this.shamProbability = shamProbability;

            Fields = new Tile[BoardWidth, BoardHeight];

            for (uint x = 0; x < BoardWidth; x++)
            {
                for (uint y = 0; y < goalAreaHeight; y++)
                {
                    Fields[x, y] = new GoalTile(TeamColor.Red, x, y);
                    Fields[x, BoardHeight - y - 1] = new GoalTile(TeamColor.Blue, x, BoardHeight - y - 1);
                }

                for (uint y = goalAreaHeight; y < taskAreaHeight + goalAreaHeight; y++)
                {
                    Fields[x, y] = new TaskTile(x, y);
                }
            }
            Pieces = new List<BoardPiece>();
        }

        public void PlaceNewPiece()
        {
            Random random = new Random();
            var taskTiles = GetTaskTiles();
            var tiles = taskTiles.Where(tile => tile.Piece == null).ToList();
            if (tiles.Count == 0) return;
            var selectedTile = tiles[random.Next(tiles.Count)];

            BoardPiece piece = new BoardPiece(NextPieceId, null,
                random.NextDouble() < shamProbability ? PieceType.Sham : PieceType.Normal,
                new Position(selectedTile.X, selectedTile.Y));
            selectedTile.Piece = piece;
            Pieces.Add(piece);
            RefreshBoardState();
        }

        public void Init(IList<GamePlayer> players, uint pieceCount, uint goalCount)
        {
            Random random = new Random();
            var blueGoalTiles = GetGoalTiles(TeamColor.Blue);
            var redGoalTiles = GetGoalTiles(TeamColor.Red);
            var goalTiles = new List<GoalTile>(blueGoalTiles);
            goalTiles.AddRange(redGoalTiles);
            var taskTiles = GetTaskTiles();

            foreach (var taskTile in taskTiles)
            {
                taskTile.Piece = null;
                taskTile.Player = null;
            }
            foreach (var goalTile in goalTiles)
            {
                goalTile.Player = null;
                goalTile.Type = GoalFieldType.NonGoal;
            }

            for (int i = 0; i < goalCount; i++)
            {
                var redNonGoalTiles = redGoalTiles.Where(tile => tile.Type == GoalFieldType.NonGoal).ToList();
                var blueNonGoalTiles = blueGoalTiles.Where(tile => tile.Type == GoalFieldType.NonGoal).ToList();
                if (redGoalTiles.Count == 0 || blueGoalTiles.Count == 0) break;

                redNonGoalTiles[random.Next(redNonGoalTiles.Count)].Type = GoalFieldType.Goal;
                blueNonGoalTiles[random.Next(blueNonGoalTiles.Count)].Type = GoalFieldType.Goal;
            }

            foreach (var gamePlayer in players)
            {
                var tiles = gamePlayer.Team == TeamColor.Blue ? blueGoalTiles : redGoalTiles;
                var freeTiles = tiles.Where(tile => tile.Player == null).ToList();
                var selectedTile = freeTiles[random.Next(freeTiles.Count)];
                selectedTile.Player = gamePlayer;
                gamePlayer.Position = new Position(selectedTile.X, selectedTile.Y);
            }

            for (int i = 0; i < pieceCount; i++)
            {
                var tiles = taskTiles.Where(tile => tile.Piece == null).ToList();
                if (tiles.Count == 0) return;
                var selectedTile = tiles[random.Next(tiles.Count)];

                BoardPiece piece = new BoardPiece(NextPieceId, null,
                    random.NextDouble() <shamProbability ? PieceType.Sham : PieceType.Normal,
                    new Position(selectedTile.X, selectedTile.Y));
                selectedTile.Piece = piece;
                Pieces.Add(piece);
            }
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

        public bool IsOccupied(Position position)
        {
            return IsOccupied(position.X, position.Y);
        }

        public bool IsOccupied(int x, int y)
        {
            return Fields[x, y].Player != null;
        }

        public bool IsValid(int x, int y)
        {
            return x >= 0 & x < BoardWidth && y >= 0 && y < BoardHeight;
        }

        public bool IsValid(Position position)
        {
            return IsValid(position.X, position.Y);
        }

        public BoardPiece FindClosestPiece(Position position)
        {
            return Pieces
                .Where(p => p.Player == null)
                .OrderBy(p => position.ManhattanDistance(p.Position))
                .FirstOrDefault();
        }

        public void MovePlayer(GamePlayer player, Position destination)
        {
            Position pos = player.Position;
            if (pos != null)
            {
                Fields[pos.X, pos.Y].Player = null;
            }
            player.Position = destination;
            Fields[destination.X, destination.Y].Player = player;
        }

        public bool IsInGoalArea(Position position)
        {
            return position.Y < GoalAreaHeight || position.Y >= (BoardHeight - GoalAreaHeight);
        }

        public List<GoalTile> GetGoalTiles(TeamColor team)
        {
            uint startHeight = team == TeamColor.Red ? 0 : BoardHeight - GoalAreaHeight;
            List<GoalTile> goalFields = new List<GoalTile>();
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < GoalAreaHeight; y++)
                {
                    goalFields.Add(Fields[x, startHeight + y] as GoalTile);
                }
            }
            return goalFields;
        }

        public bool CheckWinConditions(TeamColor team)
        {
            uint startHeight = team == TeamColor.Red ? 0 : BoardHeight - GoalAreaHeight;
            List<GoalTile> goalFields = new List<GoalTile>();
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < GoalAreaHeight; y++)
                {
                    goalFields.Add(Fields[x,startHeight+y] as GoalTile);
                }
            }

            var goals = goalFields.Where(field => field.Type == GoalFieldType.Goal).ToList();
            var discovered = goals.Where(goal => goal.Discovered).ToList();
            if (discovered.Count == goals.Count) return true;
            return false;
        }

        public bool DropPiece(BoardPiece piece, Position position)
        {
            var tile = Fields[position.X, position.Y];
            if (!(tile is TaskTile)) return false;
            TaskTile taskTile = tile as TaskTile;
            if (taskTile.Piece != null) return false;
            taskTile.Piece = piece;
            piece.Position = new Position(position.X,position.Y);
            piece.SetPlayer(null);
            return true;
        }

        public List<TaskTile> GetTaskTiles()
        {
            List<TaskTile> taskTiles = new List<TaskTile>();
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = (int)GoalAreaHeight; y < BoardHeight - GoalAreaHeight; y++)
                {
                    taskTiles.Add(Fields[x, y] as TaskTile);
                }
            }
            return taskTiles;
        }

        public void RefreshBoardState()
        {
            var taskTiles = GetTaskTiles();
            foreach (var tile in taskTiles)
            {
                var position = new Position(tile.X, tile.Y);
                var closestPiece = FindClosestPiece(position);
                if (closestPiece == null)
                {
                    tile.DistanceToPiece = -1;
                    continue;
                }
                tile.DistanceToPiece = closestPiece.Position.ManhattanDistance(position);
                tile.Piece = tile.DistanceToPiece == 0 ? closestPiece : null;
                tile.Timestamp = Time.Now;
            }
            var goalTiles = GetGoalTiles(TeamColor.Blue);
            goalTiles.AddRange(GetGoalTiles(TeamColor.Red));

            foreach (var goalTile in goalTiles)
            {
                goalTile.Timestamp = Time.Now;
            }
        }


    }
}