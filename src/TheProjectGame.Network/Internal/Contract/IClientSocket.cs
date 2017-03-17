using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal.Contract
{
    internal interface IClientSocket : IConnectionData, IReader, IWriter
    {
        void Connect(IPEndPoint endPoint);
        void Close();
    }
}
