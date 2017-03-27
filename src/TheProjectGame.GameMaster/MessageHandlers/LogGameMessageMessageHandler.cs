using System.Runtime.CompilerServices;
using Serilog;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;

[assembly: InternalsVisibleTo("TheProjectGame.GameMaster.Tests")]
namespace TheProjectGame.GameMaster.MessageHandlers
{
    class LogGameMessageMessageHanlder : MessageHandler<GameMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        public override void Handle(GameMessage message)
        {
            logger.GameEvent(GameEvent.CreateFromMessage(message, 0, Contracts.Enums.TeamColor.Red, Contracts.Enums.PlayerType.Player));
        }
    }
}
