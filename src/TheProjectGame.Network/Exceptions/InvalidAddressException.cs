using System;

namespace TheProjectGame.Network.Exceptions
{
    internal class InvalidAddressException : Exception
    {
        private const string DefaultMessage = "The given address is invalid.";

        public InvalidAddressException() : base(DefaultMessage)
        {
        }
        
    }
}
