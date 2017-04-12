using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public interface IBoard
    {
        uint BoardWidth { get; }
        uint BoardHeight { get; }
        uint TaskAreaHeight { get; }
        uint GoalAreaHeight { get; }
        Tile[,] Fields { get; }
        List<BoardPiece> Pieces { get; }

        void Init(IList<GamePlayer> players, uint pieceCount, uint goalCount);
        IEnumerable<Tile> GetNeighbourhood(int x, int y);
        bool IsOccupied(Position position);
        bool IsValid(Position position);
        BoardPiece FindClosestPiece(Position position);
        void MovePlayer(GamePlayer player, Position destination);
        bool IsInGoalArea(Position position);
        bool CheckWinConditions(TeamColor team);
        bool DropPiece(BoardPiece piece, Position position);
        void RefreshBoardState();
    }
}
