using Autofac;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Settings
{
    public static class ConfigurationBuilderExtensions
    {
        public static void RegisterOptions<TOptions>(this ContainerBuilder builder)
            where TOptions : GeneralOptions, new()
        {
            builder.Register(c => c.Resolve<OptionsParser>().GetOptions<TOptions>())
                .As<TOptions>().As<GeneralOptions>()
                .SingleInstance();
        }
    }
}
