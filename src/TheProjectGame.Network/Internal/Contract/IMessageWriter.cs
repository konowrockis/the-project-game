using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheProjectGame.Network.Internal.Contract
{
    internal interface IMessageWriter
    {
        void Send(IWriter writer, string message);
    }
}
