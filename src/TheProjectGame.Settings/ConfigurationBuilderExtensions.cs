using Autofac;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    public static class ConfigurationBuilderExtensions
    {
        public static void RegisterOptions<TOptions>(this ContainerBuilder builder)
            where TOptions : NetworkOptions, new()
        {
            builder.Register(c => c.Resolve<OptionsParser>().GetOptions<TOptions>())
                .As<TOptions>().As<NetworkOptions>()
                .SingleInstance();
        }
    }
}
