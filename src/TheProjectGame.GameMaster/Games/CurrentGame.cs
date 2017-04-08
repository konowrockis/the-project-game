using TheProjectGame.Game;
using TheProjectGame.GameMaster.MessageHandlers;

namespace TheProjectGame.GameMaster.Games
{
    interface ICurrentGame
    {
        GameState Game { get; }
        PlayersMap Players { get; }
    }

    interface IGameCreator
    {
        void SetCurrentGame(GameState game);
    }

    class CurrentGame : ICurrentGame, IGameCreator
    {
        public GameState Game { get; private set; }

        public PlayersMap Players { get; private set; }

        public void SetCurrentGame(GameState game)
        {
            Game = game;
            Players = new PlayersMap();
        }
    }
}
