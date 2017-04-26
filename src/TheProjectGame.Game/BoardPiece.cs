using System;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class BoardPiece
    {
        public ulong Id { get; private set; }
        public GamePlayer Player { get; set; }
        public PieceType Type { get; set; }
        public Position Position { get; set; }
        public DateTime Timestamp { get; set; }

        public BoardPiece(ulong id, GamePlayer player, PieceType type, Position position)
        {
            Id = id;
            Player = player;
            Type = type;
            Position = position;
        }

        public void SetPlayer(GamePlayer player)
        {
            Player = player;
        }
    }
}
