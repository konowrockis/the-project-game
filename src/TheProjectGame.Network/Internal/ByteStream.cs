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

            int move = 0;

            for (int i = 0; i < read; i++)
            {
                if (buffer[i] == ETB)
                {
                    move++;
                }
                else if (move != 0)
                {
                    buffer[i - move] = buffer[i];
                }
            }

            return read - move;
        }

        public override int ReadByte()
        {
            int b = base.ReadByte();
            return b == ETB ? ReadByte() : b;
        }
    }
}
