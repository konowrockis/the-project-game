using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    public interface IConnectionData
    {
        IPAddress Address();
        int Port();
        bool Connected { get; }
    }
}
