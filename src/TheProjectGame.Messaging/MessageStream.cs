using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public class MessageStream
    {
        private const byte ETB = 0x17;

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

                    var etb = stream.ReadByte();
                    if (ETB != etb)
                    {
                        // TODO: Exception?
                    }

                    return message;
                }
            }

            return null;
        }

        public void Write(IMessage message)
        {
            messageParser.Write(stream, message);
            stream.WriteByte(ETB);
        }
    }
}
