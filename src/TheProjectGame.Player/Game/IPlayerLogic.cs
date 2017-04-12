using TheProjectGame.Contracts;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    interface IPlayerLogic
    {
        IMessage GetNextMove(IBoard board, PlayerKnowledge knowledge);
    }
}
