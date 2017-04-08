using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class BoardPiece
    {
        public ulong Id { get; private set; }
        public GamePlayer Player { get; private set; }
        public PieceType Type { get; private set; }
    }
}
