using System;
using System.IO;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network.Internal
{
    internal class MessageHandler : IMessageHandler
    {
        private const byte ETB = 0x17;

        private string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public string Read(IReader reader)
        {
            MemoryStream buffer = new MemoryStream();
            byte[] b = new byte[1];
            while (reader.Read(b, 0, 1) > 0)
            {
                if (b[0] != ETB)
                {
                    buffer.Write(b, 0, 1);
                }
                else break;
            }
            return BytesToString(buffer.ToArray());
        }

        public void Send(IWriter writer, string message)
        {
            byte[] msgBytes = StringToBytes(message);
            byte[] data = new byte[msgBytes.Length + 1];
            Array.Copy(msgBytes, 0, data, 0, msgBytes.Length);
            data[msgBytes.Length] = ETB;
            int wrote = 0;
            while (wrote < data.Length)
            {
                wrote += writer.Write(data, wrote, data.Length - wrote);
            }
        }
    }
}
