using TheProjectGame.Contracts;

namespace TheProjectGame.Messaging
{
    public interface IMessageExecutor
    {
        void Execute<TMessage>(TMessage message) where TMessage: IMessage;
    }
}
