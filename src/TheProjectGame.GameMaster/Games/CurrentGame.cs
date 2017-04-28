using System;
using TheProjectGame.Game;

namespace TheProjectGame.GameMaster.Games
{
    public interface ICurrentGame : IGameHolder
    {
        IPlayersMap Players { get; }

        void UpdateGame();
    }

    public interface IGameCreator
    {
        void SetCurrentGame(IGameState game);
    }

    class CurrentGame : ICurrentGame, IGameCreator
    {
        public IGameState Game { get; private set; }

        public IPlayersMap Players { get; private set; }

        public void SetCurrentGame(IGameState game)
        {
            Game = game;
            Players = new PlayersMap();

            UpdateGame();
        }

        public void UpdateGame()
        {
            GameUpdated?.Invoke(Game, EventArgs.Empty);
        }

        public event EventHandler GameUpdated;
    }
}
