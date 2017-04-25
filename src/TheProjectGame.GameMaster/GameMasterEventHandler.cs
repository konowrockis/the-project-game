using System;
using System.IO;
using Serilog;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;
using TheProjectGame.Network;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster
{
    class GameMasterEventHandler : IClientEventHandler
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageReader messageReader;
        private readonly IMessageExecutor messageExecutor;
        private readonly IMessageProxyCreator proxyCreator;
        private readonly IMessageWriter messageWriter;
        private readonly GameMasterOptions options;

        public GameMasterEventHandler(IMessageReader messageReader, IMessageWriter messageWriter, 
            IMessageProxyCreator proxyCreator, IMessageExecutor messageExecutor, GameMasterOptions options)
        {
            this.messageReader = messageReader;
            this.proxyCreator = proxyCreator;
            this.messageExecutor = messageExecutor;
            this.messageWriter = messageWriter;
            this.options = options;
        }

        public void OnOpen(IConnection connection, Stream stream)
        {
            logger.Debug("Connected to host at port {@Port}", connection.Port);

            proxyCreator.SetStream(stream);
            var registerGame = new RegisterGame()
            {
                NewGameInfo = new GameInfo()
                {
                    Name = options.GameDefinition.GameName,
                    BlueTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam,
                    RedTeamPlayers = options.GameDefinition.NumberOfPlayersPerTeam
                }
            };
            messageWriter.Write(registerGame);

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