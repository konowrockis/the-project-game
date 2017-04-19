using TheProjectGame.Contracts;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public interface IPlayerLogic
    {
        IMessage GetNextMove(IBoard board, PlayerKnowledge knowledge);
    }
}
