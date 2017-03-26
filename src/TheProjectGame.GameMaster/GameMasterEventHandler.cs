using System;
using System.IO;
using System.Xml;
using Serilog;
using TheProjectGame.Contracts.Enums;
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
            logger.Debug("Connected");

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
                //logger.GameEvent(new GameEvent("test",1,1,GameEventType.Move,TeamColour.Blue, PlayerType.Leader));
                messageExecutor.Execute(message);
            }
        }

        public void OnClose(IConnectionData data)
        {
            logger.Debug("Disconnected");
        }

        public void OnError(IConnectionData data, Exception exception)
        {
            logger.Error("Error = {0}", exception);
        }
    }
}