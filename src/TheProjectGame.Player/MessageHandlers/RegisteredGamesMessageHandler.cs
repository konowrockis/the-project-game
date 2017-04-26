using System;
using Serilog;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class RegisteredGamesMessageHandler : MessageHandler<RegisteredGamesMessage>
    {
        private readonly ILogger logger = Log.ForContext<RegisteredGamesMessageHandler>();
        private readonly IMessageWriter messageWriter;
        private readonly PlayerOptions playerOptions;

        public RegisteredGamesMessageHandler(IMessageWriter messageWriter, PlayerOptions playerOptions)
        {
            this.messageWriter = messageWriter;
            this.playerOptions = playerOptions;
        }

        public override void Handle(RegisteredGamesMessage message)
        {
            var games = message.GameInfo;

            if (games.Count > 0)
            {
                int gameId = new Random().Next(games.Count);

                var response = new JoinGameMessage()
                {
                    GameName = games[gameId].Name,
                    PreferedRole = playerOptions.Role == "leader" ? Contracts.Enums.PlayerType.Leader : Contracts.Enums.PlayerType.Player,
                    PreferedTeam = playerOptions.TeamColor == "red" ? Contracts.Enums.TeamColor.Red : Contracts.Enums.TeamColor.Blue
                };
                logger.Debug("Joining game");

                // TODO: Add enums there!
                messageWriter.Write(response);
            }
            else
            {
                messageWriter.Write(new GetGamesMessage(), 100);
            }
        }
    }
}
