using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal.Contract
{
    interface IServerSocket
    {
        void Listen(int port);
        IClientSocket Accept();
    }
}
