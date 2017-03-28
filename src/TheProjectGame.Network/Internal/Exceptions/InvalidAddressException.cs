using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal.Exceptions
{
    internal class InvalidAddressException : Exception
    {
        private const string Message = "The given address is invalid.";
        public InvalidAddressException() : base(Message)
        {
        }
        
    }
}
