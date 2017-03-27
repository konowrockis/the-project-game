using TheProjectGame.Contracts;

namespace TheProjectGame.CommunicationServer.Routing
{
    public interface IGame
    {
        ulong Id { get; }
        string Name { get; }
        IClient GameMaster { get; }

        ulong BlueTeamPlayers { get; }
        ulong RedTeamPlayers { get; }
    }
}
