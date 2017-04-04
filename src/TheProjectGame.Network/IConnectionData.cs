using System.Net;

namespace TheProjectGame.Network
{
    public interface IConnectionData
    {
        IPAddress Address { get; }
        int Port { get; }
        bool Connected { get; }
    }
}
