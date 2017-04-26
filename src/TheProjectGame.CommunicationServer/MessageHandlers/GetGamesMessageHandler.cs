using System.Linq;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class GetGamesMessageHandler : MessageHandler<GetGamesMessage>
    {
        private readonly IClient currentClient;
        private readonly IGamesManager gamesManager;

        public GetGamesMessageHandler(ICurrentClient currentClient, IGamesManager gamesManager)
        {
            this.currentClient = currentClient.Value;
            this.gamesManager = gamesManager;
        }

        public override void Handle(GetGamesMessage message)
        {
            var games = gamesManager.GetGamesList()
                .Select(g => new GameInfo()
                {
                    Name = g.Name,
                    BlueTeamPlayers = g.BlueTeamPlayers,
                    RedTeamPlayers = g.RedTeamPlayers
                }).ToList();

            var response = new RegisteredGamesMessage()
            {
                GameInfo = games
            };

            currentClient.Write(response);
        }
    }
}
