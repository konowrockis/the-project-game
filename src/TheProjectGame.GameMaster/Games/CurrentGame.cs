using TheProjectGame.Game;

namespace TheProjectGame.GameMaster.Games
{
    public interface ICurrentGame : IGameHolder
    {
        IPlayersMap Players { get; }
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
        }
    }
}
