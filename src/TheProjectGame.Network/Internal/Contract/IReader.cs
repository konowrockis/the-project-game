using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network.Internal.Contract
{
    internal interface IReader
    {
        int Read(byte[] bytes, int offset, int length);
    }
}
