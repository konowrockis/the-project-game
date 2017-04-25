using Serilog;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class LogGameMessageMessageHanlder : MessageHandler<GameMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private IPlayersMap map;
        private IGameState gameState;


        public LogGameMessageMessageHanlder(ICurrentGame currentGame)
        {
            this.map = currentGame.Players;
            this.gameState = currentGame.Game;
        }

        public override void Handle(GameMessage message)
        {
            var gamePlayer = map.GetPlayer(message.PlayerGuid);
            logger.GameEvent(GameEvent.CreateFromMessage(message, gamePlayer.Id, gamePlayer.Team, gamePlayer.Role));
        }
    }
}
