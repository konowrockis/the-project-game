using System.Net.Sockets;

namespace TheProjectGame.Network.Internal
{
    public class ByteStream : NetworkStream
    {
        private const byte ETB = 0x17;

        public ByteStream(Socket s) : base(s)
        { }

        public override int Read(byte[] buffer, int offset, int size)
        {
            int read = base.Read(buffer, offset, size);

            if (read == 1 && buffer[0] == ETB)
            {
                return Read(buffer, offset, size);
            }

            return read;
        }

        public override int ReadByte()
        {
            int b = base.ReadByte();
            return b == ETB ? ReadByte() : b;
        }
    }
}
