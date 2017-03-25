using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace TheProjectGame.GameMaster.Logging
{
    internal static class LogExtensions
    {
        public static void GameEvent(this ILogger logger, GameEvent gameEvent)
        {
            logger.Verbose("{@GameEvent}",gameEvent);
        }
    }
}
