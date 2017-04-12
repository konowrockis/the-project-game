using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class GameState
    {
        public ulong Id { get; }
        public IList<GamePlayer> Players { get; }
        public Board Board { get; }

        public IEnumerable<GamePlayer> TeamPlayers(TeamColor color) => Players.Where(p => p.Team == color);

        public GameState(ulong id, uint width, uint taskAreaHeight, uint goalAreaHeight, double shamProbability)
        {
            Id = id;
            Players = new List<GamePlayer>();
            Board = new Board(width, taskAreaHeight, goalAreaHeight, shamProbability);
        }
    }
}
