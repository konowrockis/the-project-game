using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public interface IMessageHandler
    {
        void Handle(IMessage message);
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage: IMessage
    {
        void Handle(TMessage message);
    }

    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        public abstract void Handle(TMessage message);

        void IMessageHandler.Handle(IMessage message)
        {
            Handle((TMessage)message);
        }
    }
}
