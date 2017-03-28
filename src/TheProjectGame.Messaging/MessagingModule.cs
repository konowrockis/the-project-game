using Autofac;

namespace TheProjectGame.Messaging
{
    public class MessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessagesParser>().As<IMessageParser>().SingleInstance();
            builder.RegisterType<SchemaSource>().As<ISchemaSource>().InstancePerDependency();
            builder.RegisterType<MessageStream>();

            builder.RegisterType<DefaultMessageExecutor>().As<IMessageExecutor>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacMessageHandlerResolver>().As<IMessageHandlerResolver>().InstancePerLifetimeScope();

            builder.RegisterType<MessageProxy>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
