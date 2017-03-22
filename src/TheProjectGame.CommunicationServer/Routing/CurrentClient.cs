using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Messaging;

namespace TheProjectGame.CommunicationServer.Routing
{
    class CurrentClient : ICurrentClient
    {
        public IClient Value { get; set; }
    }
}
