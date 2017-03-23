using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
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
            var reader = XmlReader.Create(stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var message = messageParser.Parse(reader.Name, reader.ReadSubtree());
                    //var etb = stream.ReadByte();
                    //if (ETB != etb)
                    //{
                    // TODO: Exception?
                    //}

                    return message;
                }
            }


            return null;
        }

        public void Write(IMessage message, double delayMiliseconds = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMiliseconds)).ContinueWith((t) =>
            {
                try
                {
                    messageParser.Write(stream, message);
                    stream.WriteByte(ETB);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
        }
    }
}
