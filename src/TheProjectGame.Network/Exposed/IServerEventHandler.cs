using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    public interface IServerEventHandler : IClientEventHandler
    {
        void OnServerStart();
        void OnServerError(Exception exception);
        void OnServerStop();
    }
}
