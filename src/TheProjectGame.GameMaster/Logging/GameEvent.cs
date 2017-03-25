using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.GameMaster.Logging
{
    internal class GameEvent
    {
        public string PlayerGuid { get; private set; }
        public ulong PlayerId { get; private set; }
        public ulong GameId { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public GameEventType Type { get; private set; }
        public TeamColour Color { get; private set; }
        public PlayerType Role { get; private set; }

        public GameEvent(string playerGuid, ulong playerId, ulong gameId, GameEventType type, TeamColour color, PlayerType role)
        {
            PlayerGuid = playerGuid;
            PlayerId = playerId;
            GameId = gameId;
            TimeStamp = DateTime.Now;
            Type = type;
            Color = color;
            Role = role;
        }
    }

    public enum GameEventType
    {
        Move,
        TestPiece,
        PlacePiece,
        Victory,
        Defeat
    }

}
