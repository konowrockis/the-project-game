using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public interface IPlayerKnowledge
    {
        GamePlayer Player { get; }
        BoardPiece CarriedPiece { get; }
        string MyGuid { get; }
        IGameState GameState { get; }

        void Init(GamePlayer player, string myGuid, IGameState gameState);
        void SetCarriedPiece(BoardPiece piece);
        void ClearCarriedPiece();
    }
}
