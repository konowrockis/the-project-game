﻿using TheProjectGame.Contracts;

namespace TheProjectGame.CommunicationServer.Routing
{
    public interface IClient
    {
        ulong? GameId { get; }
        ulong PlayerId { get; }
        string PlayerGuid { get; }

        void JoinGame(ulong gameId);
        void DisconnectFromGame();

        void Write(IMessage message);
    }
}
