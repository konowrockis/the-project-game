using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class ConfirmJoiningGameMessageHandler : MessageHandler<ConfirmJoiningGame>
    {
        private readonly IMessageWriter messageWriter;
        private readonly PlayerKnowledge playerKnowledge;

        public ConfirmJoiningGameMessageHandler(IMessageWriter messageWriter, PlayerKnowledge playerKnowledge)
        {
            this.messageWriter = messageWriter;
            this.playerKnowledge = playerKnowledge;
        }

        public override void Handle(ConfirmJoiningGame message)
        {
            // Patience is a virtue

            playerKnowledge.GameState = new GameState(message.GameId);
            playerKnowledge.Guid = message.PrivateGuid;

            var playerData = message.PlayerDefinition;
            var player = new GamePlayer(playerData.Id)
            {
                Role = playerData.Type,
                Team = playerData.Team
            };

            playerKnowledge.Player = player;
        }
    }
}
