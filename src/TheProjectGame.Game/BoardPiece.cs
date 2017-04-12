using System;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Game
{
    public class BoardPiece
    {
        public ulong Id { get; private set; }
        public GamePlayer Player { get; private set; }
        public PieceType Type { get; private set; }
        public Position Position { get; set; }

        public BoardPiece(ulong id, GamePlayer player, PieceType type, Position position)
        {
            Id = id;
            Player = player;
            Type = type;
            Position = position;
        }

        public Piece ToPiece()
        {
            var piece = new Piece()
            {
                Id = Id,
                Timestamp = DateTime.Now,
                Type = Type,
            };
            if (Player != null)
            {
                piece.PlayerIdSpecified = true;
                piece.PlayerId = Player.Id;
            }
            return piece;
        }
    }
}
