using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class MessageProxy : IMessageReader, IMessageWriter, IMessageProxyCreator
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
            Task.Delay(TimeSpan.FromMilliseconds(delayMiliseconds)).ContinueWith((t) =>
            {
                try
                {
                    messageStream.Write(message);
                }
                catch { }
            });
        }
    }
}
