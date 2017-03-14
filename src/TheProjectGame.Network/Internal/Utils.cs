using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheProjectGame.Network
{
    internal class Utils
    {
        public const byte ETB = 0x17;

        public static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }
    }
}
