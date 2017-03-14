using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;using TheProjectGame.Network;

namespace TheProjectGame.Network.Internal
{
    internal class NullConnectionData : IConnectionData
    {
        public IPAddress Address()
        {
            return IPAddress.Loopback;
        }

        public int Port()
        {
            return -1;
        }
    }
}
