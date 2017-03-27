using System;
using System.IO;
using Serilog;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;
using TheProjectGame.Network;

namespace TheProjectGame.GameMaster
{
    class GameMasterEventHandler : IClientEventHandler
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageReader messageReader;
        private readonly IMessageExecutor messageExecutor;
        private readonly IMessageProxyCreator proxyCreator;
        private readonly IMessageWriter messageWriter;

        public GameMasterEventHandler(IMessageReader messageReader, IMessageWriter messageWriter, 
            IMessageProxyCreator proxyCreator, IMessageExecutor messageExecutor)
        {
            this.messageReader = messageReader;
            this.proxyCreator = proxyCreator;
            this.messageExecutor = messageExecutor;
            this.messageWriter = messageWriter;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            logger.Debug("Connected to host at port {@Port}", connection.Port());

            proxyCreator.SetStream(stream);

            messageWriter.Write(new RegisterGame()
            {
                NewGameInfo = new GameInfo()
                {
                    Name = Guid.NewGuid().ToString(),
                    BlueTeamPlayers = 10,
                    RedTeamPlayers = 10
                }
            });

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