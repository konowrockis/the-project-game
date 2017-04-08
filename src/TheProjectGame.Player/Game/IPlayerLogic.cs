using System.Runtime.Remoting.Messaging;

namespace TheProjectGame.Player.Game
{
    interface IPlayerLogic
    {
        IMessage GetNextMove();
    }
}
