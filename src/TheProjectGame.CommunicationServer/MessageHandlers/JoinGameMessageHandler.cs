﻿using System.Linq;
using System.Runtime.CompilerServices;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Messaging;

[assembly: InternalsVisibleTo("TheProjectGame.CommunicationServer.Tests")]
namespace TheProjectGame.CommunicationServer.MessageHandlers
{
    
    class JoinGameMessageHandler : MessageHandler<JoinGame>
    {
        private readonly IClient currentClient;
        private readonly IGamesManager gamesManager;

        public JoinGameMessageHandler(ICurrentClient currentClient, IGamesManager gamesManager)
        {
            this.currentClient = currentClient.Value;
            this.gamesManager = gamesManager;
        }

        public override void Handle(JoinGame message)
        {
            var game = gamesManager.GetGameByName(message.GameName);
            if (game == null)
            {
                var response = new RejectJoiningGame()
                {
                    GameName = message.GameName,
                    PlayerId = 0
                };

                currentClient.Write(response);
            }
            else
            {
                message.PlayerId = currentClient.PlayerId;
                game.GameMaster.Write(message);
            }
        }
    }
}
