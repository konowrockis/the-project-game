using System.Linq;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class GetGamesMessageHandler : MessageHandler<GetGames>
    {
        private readonly IClient currentClient;
        private readonly IGamesManager gamesManager;

        public GetGamesMessageHandler(ICurrentClient currentClient, IGamesManager gamesManager)
        {
            this.currentClient = currentClient.Value;
            this.gamesManager = gamesManager;
        }

        public override void Handle(GetGames message)
        {
            var games = gamesManager.GetGamesList()
                .Select(g => new GameInfo()
                {
                    Name = g.Name,
                    BlueTeamPlayers = g.BlueTeamPlayers,
                    RedTeamPlayers = g.RedTeamPlayers
                }).ToList();

            var response = new RegisteredGames()
            {
                GameInfo = games
            };

            currentClient.Write(response);
        }
    }
}
