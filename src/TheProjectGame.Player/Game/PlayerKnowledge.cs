using System;
using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public class PlayerKnowledge : IPlayerKnowledge
    {
        public GamePlayer Player { get; private set; }
        public BoardPiece CarriedPiece { get; private set; }
        public string MyGuid { get; private set; }
        public IGameState GameState { get; private set; }

        public void Init(GamePlayer player, string myGuid, IGameState gameState)
        {
            Player = player;
            MyGuid = myGuid;
            GameState = gameState;
        }

        public void SetCarriedPiece(BoardPiece piece)
        {
            CarriedPiece = piece;
        }

        public void ClearCarriedPiece()
        {
            CarriedPiece = null;
        }
    }
}
