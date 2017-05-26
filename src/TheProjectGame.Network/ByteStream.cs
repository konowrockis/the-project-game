using System.IO;
using System.Net.Sockets;
using System.Text;

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
                    System.Console.Write(Encoding.UTF8.GetString(b));
                }
                else if (messageBuffer.Length != 0)
                {
                    messageBuffer.Position = 0;
                    System.Console.WriteLine();
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
