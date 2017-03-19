using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal.Contract
{
    internal interface IClientSocket : IConnectionData
    {
        Socket RawSocket { get; }
        void Connect(IPEndPoint endPoint);
        void Close();
    }
}
