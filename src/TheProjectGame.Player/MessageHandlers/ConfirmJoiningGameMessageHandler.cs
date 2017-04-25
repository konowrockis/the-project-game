using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class ConfirmJoiningGameMessageHandler : MessageHandler<ConfirmJoiningGame>
    {
        private readonly IMessageWriter messageWriter;
        private readonly IPlayerKnowledge playerKnowledge;

        public ConfirmJoiningGameMessageHandler(
            IMessageWriter messageWriter, 
            IPlayerKnowledge playerKnowledge)
        {
            this.messageWriter = messageWriter;
            this.playerKnowledge = playerKnowledge;
        }

        public override void Handle(ConfirmJoiningGame message)
        {
            // Patience is a virtue

            var playerData = message.PlayerDefinition;
            var player = new GamePlayer(playerData.Id)
            {
                Role = playerData.Type,
                Team = playerData.Team
            };

            playerKnowledge.Init(player, message.PrivateGuid, new GameState(message.GameId));
        }
    }
}
