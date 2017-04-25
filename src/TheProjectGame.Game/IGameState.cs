using System.Collections.Generic;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public interface IGameState
    {
        ulong Id { get; }
        List<GamePlayer> Players { get; set; }
        IBoard Board { get; set; }
        IEnumerable<GamePlayer> TeamPlayers(TeamColor color);
    }
}
