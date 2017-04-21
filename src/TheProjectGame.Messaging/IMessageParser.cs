using System.IO;
using System.Xml;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public interface IMessageParser
    {
        IMessage Parse(string messageName, XmlReader reader);
        IMessage Parse(string messageName, Stream stream);
        void Write(Stream stream, IMessage message);
    }
}
