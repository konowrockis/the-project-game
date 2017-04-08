using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class GamePlayer
    {
        public ulong Id { get; }

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public TeamColor Team { get; private set; }
        public PlayerType Role { get; private set; }

        public GamePlayer(ulong id)
        {
            Id = id;
        }

        public void InitTeam(TeamColor team, PlayerType role)
        {
            Team = team;
            Role = role;
        }
    }
}
