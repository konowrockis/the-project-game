using System;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class RegisteredGamesMessageHandler : MessageHandler<RegisteredGames>
    {
        private readonly IMessageWriter messageWriter;
        private readonly PlayerOptions playerOptions;

        public RegisteredGamesMessageHandler(IMessageWriter messageWriter, PlayerOptions playerOptions)
        {
            this.messageWriter = messageWriter;
            this.playerOptions = playerOptions;
        }

        public override void Handle(RegisteredGames message)
        {
            var games = message.GameInfo;

            if (games.Count > 0)
            {
                int gameId = new Random().Next(games.Count);

                var response = new JoinGame()
                {
                    GameName = games[gameId].Name,
                    PreferedRole = playerOptions.Role == "leader" ? Contracts.Enums.PlayerType.Leader : Contracts.Enums.PlayerType.Player,
                    PreferedTeam = playerOptions.TeamColor == "red" ? Contracts.Enums.TeamColour.Red : Contracts.Enums.TeamColour.Blue
                };
                Console.WriteLine("Joining game");

                // TODO: Add enums there!
                messageWriter.Write(response);
            }
            else
            {
                messageWriter.Write(new GetGames(), 100);
            }
        }
    }
}
