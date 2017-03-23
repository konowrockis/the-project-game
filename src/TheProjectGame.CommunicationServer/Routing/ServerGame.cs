using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.CommunicationServer.Routing
{
    class ServerGame : IGame
    {
        public ulong Id { get; private set; }
        public string Name { get; private set; }
        public IClient GameMaster { get; private set; }
        public ulong BlueTeamPlayers { get; private set; }
        public ulong RedTeamPlayers { get; private set; }

        public ServerGame(ulong id, string name, IClient gameMaster, ulong blueTeamPlayers, ulong redTeamPlayers)
        {
            Id = id;
            Name = name;
            GameMaster = gameMaster;
            BlueTeamPlayers = blueTeamPlayers;
            RedTeamPlayers = redTeamPlayers;
        }
    }
}
