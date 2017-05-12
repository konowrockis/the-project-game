using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public class MessageStream
    {
        private const byte ETB = 0x17;

        private readonly Stream stream;
        private readonly XmlDocument document;
        private readonly IMessageParser messageParser;
        private readonly XmlReaderSettings readerSettings;

        public delegate MessageStream Factory(Stream stream);

        public MessageStream(Stream stream, IMessageParser messageParser, ISchemaSource schemaSource)
        {
            this.stream = stream;
            this.messageParser = messageParser;

            readerSettings = GetXmlReaderSettings(schemaSource);
            document = new XmlDocument();
        }

        public IMessage Read()
        {
            var reader = XmlReader.Create(stream, readerSettings);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var message = messageParser.Parse(reader.Name, reader.ReadSubtree());

                    return message;
                }
            }

            return null;
        }

        public void Write(IMessage message, double delayMiliseconds = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMiliseconds)).ContinueWith((t) =>
            {
                    messageParser.Write(stream, message);
            });
        }

        private XmlReaderSettings GetXmlReaderSettings(ISchemaSource schemaSource)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            /*using (var schema = schemaSource.GetSchema())
            {
                schemaSet.Add(XmlSchema.Read(schema, null));
            }*/

            return new XmlReaderSettings
            {
                //ValidationType = ValidationType.Schema,
                //Schemas = schemaSet,

                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true
            };
        }
    }
}
