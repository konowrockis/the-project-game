using System;
using System.IO;

namespace TheProjectGame.Network
{
    public interface IClientEventHandler
    {
        void OnOpen(IConnection connection, Stream stream);
        void OnClose(IConnectionData data);
        void OnError(IConnectionData data, Exception exception);
    }
}
