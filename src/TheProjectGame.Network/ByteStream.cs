using System.IO;
using System.Net.Sockets;

namespace TheProjectGame.Network
{
    class ByteStream : NetworkStream
    {
        private const byte ETB = 0x17;
        private MemoryStream messageBuffer = new MemoryStream();

        public ByteStream(Socket s) : base(s)
        { }

        private void StreamMessage()
        {
            if (messageBuffer.Position != messageBuffer.Length) return;
            messageBuffer.SetLength(0);
            byte[] b = new byte[1];
            while (base.Read(b, 0, 1) > 0)
            {
                if (b[0] != ETB)
                {
                    messageBuffer.Write(b, 0, 1);
                }
                else if (messageBuffer.Length != 0)
                {
                    messageBuffer.Position = 0;
                    return;
                }
                else
                {
                    WriteByte(ETB);
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int size)
        {
            StreamMessage();
            return messageBuffer.Read(buffer, offset, size);
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            base.Write(buffer, offset, size);
            System.Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
        }
    }
}
