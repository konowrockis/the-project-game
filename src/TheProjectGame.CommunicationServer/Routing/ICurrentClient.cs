using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.CommunicationServer.Routing
{
    interface ICurrentClient
    {
        IClient Value { get; set; }
    }
}
