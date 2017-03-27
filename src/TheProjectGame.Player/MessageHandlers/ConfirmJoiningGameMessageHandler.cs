﻿using System.Runtime.CompilerServices;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;

[assembly: InternalsVisibleTo("TheProjectGame.Player.Tests")]
namespace TheProjectGame.Player.MessageHandlers
{
    class ConfirmJoiningGameMessageHandler : MessageHandler<ConfirmJoiningGame>
    {
        private readonly IMessageWriter messageWriter;

        public ConfirmJoiningGameMessageHandler(IMessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public override void Handle(ConfirmJoiningGame message)
        {

        }
    }
}
