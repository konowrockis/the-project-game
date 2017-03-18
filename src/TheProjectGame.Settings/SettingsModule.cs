using System;
using Autofac;

namespace TheProjectGame.Settings
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OptionsParser>()
                .WithParameter("args", Environment.GetCommandLineArgs())
                .AsSelf().SingleInstance();
        }
    }
}
