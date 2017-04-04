using System.IO;
using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public interface IMessageReader
    {
        IMessage Read();
    }

    public interface IMessageWriter
    {
        void Write(IMessage message, double delayMiliseconds = 0);
    }

    public interface IMessageProxyCreator
    {
        void SetStream(Stream messageStream);
    }

    class MessageProxy : IMessageReader, IMessageWriter, IMessageProxyCreator
    {
        private readonly MessageStream.Factory messageStreamFactory;

        private MessageStream messageStream;

        public MessageProxy(MessageStream.Factory messageStreamFactory)
        {
            this.messageStreamFactory = messageStreamFactory;
        }

        public void SetStream(Stream messageStream)
        {
            this.messageStream = messageStreamFactory(messageStream);
        }

        public IMessage Read() => messageStream.Read();

        public void Write(IMessage message, double delayMiliseconds)
        {
            messageStream.Write(message, delayMiliseconds);
        }
    }
}
