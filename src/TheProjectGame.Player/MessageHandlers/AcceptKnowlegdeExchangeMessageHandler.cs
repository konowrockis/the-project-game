using System.Linq;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class AcceptKnowlegdeExchangeMessageHandler : MessageHandler<AcceptExchangeRequestMessage>
    {
        private readonly IMessageWriter messageWriter;
        private readonly IPlayerKnowledge playerKnowledge;

        public AcceptKnowlegdeExchangeMessageHandler(
            IMessageWriter messageWriter,
            IPlayerKnowledge playerKnowledge)
        {
            this.messageWriter = messageWriter;
            this.playerKnowledge = playerKnowledge;
        }
        public override void Handle(AcceptExchangeRequestMessage message)
        {
            SendData(message);
        }

        private void SendData(AcceptExchangeRequestMessage message)
        {
            var taskTiles = playerKnowledge.Game.Board.Fields.OfType<TaskTile>().ToList()
                .Select(t => new TaskField()
                {
                    DistanceToPiece = t.DistanceToPiece,
                    PieceId = t.Piece?.Id ?? 0,
                    PieceIdSpecified = t.Piece != null,
                    Timestamp = t.Timestamp,
                    PlayerId = t.Player?.Id ?? 0,
                    PlayerIdSpecified = t.Player != null,
                    X = t.X,
                    Y = t.Y
                })
                .ToList();

            var goalTiles = playerKnowledge.Game.Board.Fields.OfType<GoalTile>().ToList()
                .Select(t => new GoalField()
                {
                    Type = t.Type,
                    Team = t.Team,
                    Timestamp = t.Timestamp,
                    PlayerId = t.Player?.Id ?? 0,
                    PlayerIdSpecified = t.Player != null,
                    X = t.X,
                    Y = t.Y
                })
                .ToList();

            var pieces = playerKnowledge.Game.Board.Pieces
                .Select(p => new Piece()
                {
                    Id = p.Id,
                    Type = p.Type,
                    Timestamp = p.Timestamp,
                    PlayerId = p.Player?.Id ?? 0,
                    PlayerIdSpecified = p.Player != null
                })
                .ToList();

            var data = new DataMessage()
            {
                TaskFields = taskTiles,
                GoalFields = goalTiles,
                Pieces = pieces,
                PlayerId = message.SenderPlayerId
            };

            messageWriter.Write(data);
        }
    }
}
