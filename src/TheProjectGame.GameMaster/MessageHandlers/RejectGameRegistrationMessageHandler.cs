﻿using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class RejectGameRegistrationMessageHandler : MessageHandler<RejectGameRegistration>
    {
        private readonly IMessageWriter messageWriter;
        private readonly GameMasterOptions options;

        public RejectGameRegistrationMessageHandler(IMessageWriter messageWriter, GameMasterOptions options)
        {
            this.messageWriter = messageWriter;
            this.options = options;
        }

        public override void Handle(RejectGameRegistration message)
        {
            var response = new RegisterGame()
            {
                NewGameInfo = new GameInfo()
                {
                    Name = options.GameDefinition.GameName,
                    BlueTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam,
                    RedTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam
                }
            };

            messageWriter.Write(response, options.RetryRegisterGameInterval);
        }
    }
}
