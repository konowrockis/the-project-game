using System;
using System.IO;
using System.Net.Sockets;

namespace TheProjectGame.Network
{
    public interface IClientEventHandler
    {
        void OnOpen(IConnection connection, Stream stream);
        void OnClose(IConnectionData data);
        void OnError(IConnectionData data, Exception exception);
    }
}
