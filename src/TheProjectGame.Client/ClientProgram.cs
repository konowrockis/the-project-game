using System;
using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings;

namespace TheProjectGame.Client
{
    public abstract class ClientProgram<TEventHandler> : GameProgram<ClientNetworkModule<TEventHandler>> 
        where TEventHandler: IClientEventHandler 
    { }
}
