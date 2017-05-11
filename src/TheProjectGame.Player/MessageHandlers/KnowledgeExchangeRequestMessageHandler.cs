using System.Linq;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class KnowledgeExchangeRequestMessageHandler : MessageHandler<KnowledgeExchangeRequestMessage>
    {
        private readonly IMessageWriter messageWriter;
        private readonly IPlayerLogic playerLogic;
        private readonly IPlayerKnowledge playerKnowledge;

        public KnowledgeExchangeRequestMessageHandler(
            IMessageWriter messageWriter,
            IPlayerKnowledge playerKnowledge,
            IPlayerLogic playerLogic)
        {
            this.messageWriter = messageWriter;
            this.playerLogic = playerLogic;
            this.playerKnowledge = playerKnowledge;
        }

        public override void Handle(KnowledgeExchangeRequestMessage message)
        {
            if (!playerLogic.ShouldExchangeKnowledge())
            {
                RejectKnowledgeExchange(message);
            }
            else
            {
                SendData(message);
                SendAuthorization(message);
            }
        }

        private void RejectKnowledgeExchange(KnowledgeExchangeRequestMessage message)
        {
            var response = new RejectKnowledgeExchangeMessage()
            {
                Permanent = false,
                PlayerId = message.SenderPlayerId,
                SenderPlayerId = message.PlayerId
            };

            messageWriter.Write(response);
        }

        private void SendData(KnowledgeExchangeRequestMessage message)
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

        private void SendAuthorization(KnowledgeExchangeRequestMessage message)
        {
            var response = new AuthorizeKnowledgeExchangeMessage()
            {
                GameId = playerKnowledge.Game.Id,
                PlayerGuid = playerKnowledge.MyGuid,
                WithPlayerId = message.SenderPlayerId
            };

            messageWriter.Write(response);
        }
    }
}
