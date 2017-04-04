using System.Net;

namespace TheProjectGame.Network.Client
{
    class VoidConnectionData : IConnectionData
    {
        public IPAddress Address => IPAddress.Loopback;

        public int Port => -1;

        public bool Connected => false;
    }
}
