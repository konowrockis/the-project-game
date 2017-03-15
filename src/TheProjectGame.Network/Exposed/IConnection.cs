using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    public interface IConnection : IConnectionData
    {
        void Send(string message, long delayMillis=0);
        void Close();
    }
}
