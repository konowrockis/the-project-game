using System.Linq;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class KnowledgeExchangeRequestMessageHandler : MessageHandler<KnowledgeExchangeRequest>
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

        public override void Handle(KnowledgeExchangeRequest message)
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

        private void RejectKnowledgeExchange(KnowledgeExchangeRequest message)
        {
            var response = new RejectKnowledgeExchange()
            {
                Permanent = false,
                PlayerId = message.SenderPlayerId,
                SenderPlayerId = message.PlayerId
            };

            messageWriter.Write(response);
        }

        private void SendData(KnowledgeExchangeRequest message)
        {
            var taskTiles = playerKnowledge.GameState.Board.Fields.OfType<TaskTile>().ToList()
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

            var goalTiles = playerKnowledge.GameState.Board.Fields.OfType<GoalTile>().ToList()
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

            var pieces = playerKnowledge.GameState.Board.Pieces
                .Select(p => new Piece()
                {
                    Id = p.Id,
                    Type = p.Type,
                    Timestamp = p.Timestamp,
                    PlayerId = p.Player?.Id ?? 0,
                    PlayerIdSpecified = p.Player != null
                })
                .ToList();

            var data = new Data()
            {
                TaskFields = taskTiles,
                GoalFields = goalTiles,
                Pieces = pieces,
                PlayerId = message.SenderPlayerId
            };

            messageWriter.Write(data);
        }

        private void SendAuthorization(KnowledgeExchangeRequest message)
        {
            var response = new AuthorizeKnowledgeExchange()
            {
                GameId = playerKnowledge.GameState.Id,
                PlayerGuid = playerKnowledge.MyGuid,
                WithPlayerId = message.SenderPlayerId
            };

            messageWriter.Write(response);
        }
    }
}
