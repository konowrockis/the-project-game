using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public class MessageStream
    {
        private readonly Stream stream;
        private readonly XmlDocument document;
        private readonly IMessageParser messageParser;

        public delegate MessageStream Factory(Stream stream);

        public MessageStream(Stream stream, IMessageParser messageParser)
        {
            this.stream = stream;
            this.messageParser = messageParser;
            this.document = new XmlDocument();
        }

        public IMessage Read()
        {
            XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings());

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var message = messageParser.Parse(reader.Name, reader.ReadSubtree());

                    stream.ReadByte();

                    return message;
                }
            }

            return null;
        }

        public void Write(IMessage message, double delayMillis = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMillis)).ContinueWith((t) =>
            {
                try
                {
                    messageParser.Write(stream, message);
                    stream.WriteByte(0x17);
                }
                catch { }
            });
        }
    }
}
