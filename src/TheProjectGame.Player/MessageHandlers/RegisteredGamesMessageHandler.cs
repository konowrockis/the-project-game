using System;
using System.Linq;
using Serilog;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class RegisteredGamesMessageHandler : MessageHandler<RegisteredGames>
    {
        private readonly ILogger logger = Log.ForContext<RegisteredGamesMessageHandler>();
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

            if (games.FirstOrDefault(g => g.Name == playerOptions.NameOfTheGame) == null)
            {
                logger.Debug("Game not found");
                messageWriter.Write(new GetGames(),playerOptions.RetryJoinGameInterval);
                return;
            }

            var response = new JoinGame()
            {
                GameName = playerOptions.NameOfTheGame,
                PreferedRole = playerOptions.Role == "leader" ? Contracts.Enums.PlayerType.Leader : Contracts.Enums.PlayerType.Player,
                PreferedTeam = playerOptions.TeamColor == "red" ? Contracts.Enums.TeamColor.Red : Contracts.Enums.TeamColor.Blue
            };
            logger.Debug("Joining game");

            // TODO: Add enums there!
            messageWriter.Write(response);
        }
    }
}
