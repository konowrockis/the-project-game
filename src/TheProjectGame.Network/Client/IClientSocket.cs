using System.Net;
using System.Net.Sockets;

namespace TheProjectGame.Network.Client
{
    internal interface IClientSocket : IConnectionData
    {
        Socket RawSocket { get; }
        void Connect(IPEndPoint endPoint);
        void Close();
    }
}
