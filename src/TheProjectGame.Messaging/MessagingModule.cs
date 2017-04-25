using Autofac;
using TheProjectGame.Messaging.Autofac;
using TheProjectGame.Messaging.Default;

namespace TheProjectGame.Messaging
{
    public class MessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacMessageHandlerResolver>().As<IMessageHandlerResolver>().InstancePerLifetimeScope();

            builder.RegisterType<DefaultMessagesParser>().As<IMessageParser>().SingleInstance();
            builder.RegisterType<DefaultSchemaSource>().As<ISchemaSource>().InstancePerDependency();
            builder.RegisterType<DefaultMessageExecutor>().As<IMessageExecutor>().InstancePerLifetimeScope();

            builder.RegisterType<MessageStream>();
            builder.RegisterType<MessageProxy>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
