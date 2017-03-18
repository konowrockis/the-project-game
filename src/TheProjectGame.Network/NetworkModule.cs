using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Contract;

namespace TheProjectGame.Network
{
    public abstract class NetworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageHandler>().As<IMessageHandler>().SingleInstance();
        }
    }
}
