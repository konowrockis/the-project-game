using System;

namespace TheProjectGame.Game
{
    public interface IGameHolder
    {
        IGameState Game { get; }

        event EventHandler GameUpdated;
    }
}
