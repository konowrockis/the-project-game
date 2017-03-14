using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    public interface IClientEventHandler
    {
        void OnMessage(string message, IConnection connection);
        void OnOpen(IConnection connection);
        void OnClose(IConnectionData data);
        void OnError(IConnectionData data, Exception exception);
    }
}
