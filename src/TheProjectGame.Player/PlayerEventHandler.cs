using System;
using System.IO;
using Serilog;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.Player
{
    class PlayerEventHandler : IClientEventHandler
    {
        private readonly ILogger logger = Log.ForContext<PlayerEventHandler>();

        private readonly IMessageReader messageReader;
        private readonly IMessageExecutor messageExecutor;
        private readonly IMessageProxyCreator proxyCreator;
        private readonly IMessageWriter messageWriter;

        public PlayerEventHandler(IMessageReader messageReader, IMessageWriter messageWriter, IMessageProxyCreator proxyCreator, IMessageExecutor messageExecutor)
        {
            this.messageReader = messageReader;
            this.proxyCreator = proxyCreator;
            this.messageExecutor = messageExecutor;
            this.messageWriter = messageWriter;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            logger.Debug("Connected to host at port {@Port}", connection.Port);

            proxyCreator.SetStream(stream);

            messageWriter.Write(new GetGamesMessage());

            while (true)
            {
                var message = messageReader.Read();

                messageExecutor.Execute(message);
            }
        }

        public void OnClose(IConnectionData data)
        {
            logger.Debug("Disconnected from host");
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            logger.Fatal("Received fatal exception {@Exception}", exception);
        }
    }
}
