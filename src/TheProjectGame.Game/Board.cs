using System;
using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public class Board
    {
        public uint BoardWidth { get; }
        public uint BoardHeight { get; }
        public uint TaskAreaHeight { get; }
        public uint GoalAreaHeight { get; }
        public Tile[,] Fields { get; }
        public List<BoardPiece> Pieces { get; private set; }

        private uint _pieceId = 1;

        private uint NextPieceId
        {
            get
            {
                uint val = _pieceId;
                _pieceId++;
                return val;
            }
        }

        public Board(uint width, uint taskAreaHeight, uint goalAreaHeight)
        {
            BoardWidth = width;
            TaskAreaHeight = taskAreaHeight;
            GoalAreaHeight = goalAreaHeight;
            BoardHeight = taskAreaHeight + goalAreaHeight*2;

            Fields = new Tile[BoardWidth, BoardHeight];

            for (uint x = 0; x < BoardWidth; x++)
            {
                for (uint y = 0; y < goalAreaHeight; y++)
                {
                    Fields[x, y] = new GoalTile(TeamColor.Red, x, y);
                    Fields[x, BoardHeight - y - 1] = new GoalTile(TeamColor.Blue, x, y);
                }

                for (uint y = goalAreaHeight; y < taskAreaHeight + goalAreaHeight; y++)
                {
                    Fields[x, y] = new TaskTile(x, y);
                }
            }
            Pieces = new List<BoardPiece>();
        }

        public void Init(IList<GamePlayer> players, uint pieceCount)
        {
            Random random = new Random();
            var blueGoalTiles = GetGoalTiles(TeamColor.Blue);
            var redGoalTiles = GetGoalTiles(TeamColor.Red);
            var taskTiles = GetTaskTiles();

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
                    random.Next(2) == 1 ? PieceType.Normal : PieceType.Sham,
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
            Fields[pos.X, pos.Y].Player = null;
            player.Position = destination;
            Fields[destination.X, destination.Y].Player = player;
        }

        public bool IsInGoalArea(Position position)
        {
            return position.Y < GoalAreaHeight || position.Y >= (BoardHeight - GoalAreaHeight);
        }

        private List<GoalTile> GetGoalTiles(TeamColor team)
        {
            uint startHeight = team == TeamColor.Blue ? 0 : BoardHeight - GoalAreaHeight;
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

        private List<TaskTile> GetTaskTiles()
        {
            List<TaskTile> taskTiles = new List<TaskTile>();
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = (int) GoalAreaHeight; y < BoardHeight - GoalAreaHeight; y++)
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
                var position = new Position(tile.X,tile.Y);
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