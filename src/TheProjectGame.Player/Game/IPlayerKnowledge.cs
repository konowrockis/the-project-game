using TheProjectGame.Game;

namespace TheProjectGame.Player.Game
{
    public interface IPlayerKnowledge : IGameHolder
    {
        GamePlayer Player { get; }
        BoardPiece CarriedPiece { get; }
        string MyGuid { get; }

        void Init(GamePlayer player, string myGuid, IGameState gameState);
        void SetCarriedPiece(BoardPiece piece);
        void ClearCarriedPiece();
    }
}
