using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    class RegisterGameMessageHandler : MessageHandler<RegisterGameMessage>
    {
        private readonly IClient currentClient;
        private readonly IGamesManager gamesManager;

        public RegisterGameMessageHandler(ICurrentClient currentClient, IGamesManager gamesManager)
        {
            this.currentClient = currentClient.Value;
            this.gamesManager = gamesManager;
        }

        public override void Handle(RegisterGameMessage message)
        {
            var gameInfo = message.NewGameInfo;

            if (gamesManager.GetGameByName(gameInfo.Name) != null)
            {
                var game = gamesManager.GetGameByName(gameInfo.Name);

                if (currentClient.GameId == game.Id)
                {
                    gamesManager.Remove(game);
                }
                else
                {
                    var response = new RejectGameRegistrationMessage()
                    {
                        GameName = gameInfo.Name
                    };
                    currentClient.Write(response);
                    return;
                }
            }
            RegisterGame(gameInfo);
        }

        private void RegisterGame(GameInfo gameInfo)
        {
            var id = gamesManager.GetNewGameId();
            var game = new ServerGame(id, gameInfo.Name, currentClient, gameInfo.BlueTeamPlayers, gameInfo.RedTeamPlayers);

            gamesManager.Add(game);
            var response = new ConfirmGameRegistrationMessage()
            {
                GameId = id
            };

            currentClient.JoinGame(id);
            currentClient.Write(response);
        }

    }
}
