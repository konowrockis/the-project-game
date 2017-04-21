using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Game
{
    public class GameLogicException : Exception
    {
        public GameLogicException(string message) : base(message)
        {
        }

        public GameLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
