using TheProjectGame.Contracts.Enums;

namespace TheProjectGame.Game
{
    public class GamePlayer
    {
        public ulong Id { get; }

        public Position Position { get; set; }
        public TeamColor Team { get; private set; }
        public PlayerType Role { get; private set; }

        public GamePlayer(ulong id)
        {
            Id = id;
            Position = new Position(0,0);
        }

        public GamePlayer()
        {
            
        }

        public void InitTeam(TeamColor team, PlayerType role)
        {
            Team = team;
            Role = role;
        }
    }
}
