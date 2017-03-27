using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.GameMaster.Logging
{
    internal class GameEvent
    {
        public string PlayerGuid { get; private set; }
        public ulong PlayerId { get; private set; }
        public ulong GameId { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public GameEventType Type { get; private set; }
        public TeamColor Color { get; private set; }
        public PlayerType Role { get; private set; }

        public static GameEvent CreateFromMessage(GameMessage message, ulong playerId, TeamColor color, PlayerType role)
        {
            return new GameEvent()
            {
                PlayerGuid = message.PlayerGuid,
                PlayerId = playerId,
                GameId = message.GameId,
                TimeStamp = DateTime.Now,
                Type = GetEventType(message),
                Color = color,
                Role = role
            };
        }

        public static GameEvent CreateVictory(string playerGuid, ulong gameId, ulong playerId, TeamColor color, PlayerType role)
        {
            return CreateSummary(playerGuid, gameId, playerId, color, role, true);
        }

        public static GameEvent CreateDefeat(string playerGuid, ulong gameId, ulong playerId, TeamColor color, PlayerType role)
        {
            return CreateSummary(playerGuid, gameId, playerId, color, role, false);
        }

        private static GameEvent CreateSummary(string playerGuid, ulong gameId, ulong playerId, TeamColor color, PlayerType role, bool isVictory)
        {
            return new GameEvent()
            {
                PlayerGuid = playerGuid,
                PlayerId = playerId,
                GameId = gameId,
                TimeStamp = DateTime.Now,
                Type = isVictory ? GameEventType.Victory : GameEventType.Defeat,
                Color = color,
                Role = role
            };
        }

        private static GameEventType GetEventType(GameMessage message)
        {
            if (message is Move) return GameEventType.Move;
            if (message is TestPiece) return GameEventType.TestPiece;
            if (message is PlacePiece) return GameEventType.PlacePiece;
            if (message is Discover) return GameEventType.Discover;
            if (message is PickUpPiece) return GameEventType.PickUpPiece;

            throw new Exception("Invalid message type");
        }
    }

    public enum GameEventType
    {
        Move,
        TestPiece,
        PlacePiece,
        Discover,
        PickUpPiece,
        Victory,
        Defeat
    }

}
