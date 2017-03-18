using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace TheProjectGame.Settings
{
    public static class ConfigurationBuilderExtensions
    {
        public static void RegisterOptions<TOptions>(this ContainerBuilder builder)
            where TOptions : class, new()
        {
            builder.Register(c => c.Resolve<OptionsParser>().GetOptions<TOptions>())
                .As<IOptions<TOptions>>()
                .SingleInstance();
        }
    }
}
