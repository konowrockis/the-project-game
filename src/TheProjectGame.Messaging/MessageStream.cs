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
            MemoryStream mstream = new MemoryStream();
            byte[] buffer = new byte[1];

            while (true)
            {
                int read = stream.Read(buffer, 0, 1);
                if (buffer[0] == ETB && mstream.Length!=0)
                {
                    byte []msg = mstream.ToArray();
                    var stringmsg = Encoding.UTF8.GetString(msg);

                    stringmsg = stringmsg.Replace("\n", "");
                    stringmsg = stringmsg.Replace("\r", "");

                    var header = "<?xml version=\"1.0\"?>";
                    var pattern = @"\<\?xml version=" + "\"1\\.0\"" + @"\?\>\<([A-Za-z]*)";

                    Regex r = new Regex(pattern, RegexOptions.Multiline);

                    var match = r.Match(stringmsg);

                    var groups = match.Groups;

                    stringmsg = stringmsg.Replace(header, "");

                    MemoryStream messageStream = new MemoryStream();
                    var bytes = Encoding.UTF8.GetBytes(stringmsg);
                    messageStream.Write(bytes, 0, bytes.Length);
                    messageStream.Position = 0;

                    return messageParser.Parse(groups[1].Value, messageStream);


                    /*mstream.Position = 0;
                    var reader = XmlReader.Create(mstream, readerSettings);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var message = messageParser.Parse(reader.Name, reader.ReadSubtree());

                            return message;
                        }
                    }*/
                    /*try
                    {
                        reader.Read();
                        reader.Read();
                    } catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                    return messageParser.Parse(reader.Name, reader.ReadSubtree());*/
                }
                if (buffer[0] == ETB) continue;
                mstream.Write(buffer, 0, 1);
            }
            /*var reader = XmlReader.Create(stream, readerSettings);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var message = messageParser.Parse(reader.Name, reader.ReadSubtree());

                    return message;
                }
            }*/

            return null;
        }
        private static object Lock = new object();

        public void Write(IMessage message, double delayMiliseconds = 0)
        {
            Task.Delay(TimeSpan.FromMilliseconds(delayMiliseconds)).ContinueWith((t) =>
            {
                lock (Lock)
                {
                    messageParser.Write(stream, message);
                    stream.WriteByte(ETB);
                }
            });
        }

        private XmlReaderSettings GetXmlReaderSettings(ISchemaSource schemaSource)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            using (var schema = schemaSource.GetSchema())
            {
                schemaSet.Add(XmlSchema.Read(schema, null));
            }

            return new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaSet,
                
                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true
            };
        }
    }
}
