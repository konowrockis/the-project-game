using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public interface IGameState
    {
        ulong Id { get; }
        IList<GamePlayer> Players { get; }
        IBoard Board { get; }
        IEnumerable<GamePlayer> TeamPlayers(TeamColor color);
    }
}
