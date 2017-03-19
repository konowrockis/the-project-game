using Autofac;

namespace TheProjectGame.Messaging
{
    public class MessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessagesParser>().As<IMessageParser>().SingleInstance();
            builder.RegisterType<MessageStream>();

            builder.RegisterType<DefaultMessageExecutor>().As<IMessageExecutor>();
            builder.RegisterType<AutofacMessageHandlerResolver>().As<IMessageHandlerResolver>().SingleInstance();

            builder.RegisterType<MessageProxy>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
