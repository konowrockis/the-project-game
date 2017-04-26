using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Player.MessageHandlers
{
    class DataMessageHandler : MessageHandler<DataMessage>
    {
        private readonly ILogger logger = Log.ForContext<DataMessageHandler>();

        private readonly IPlayerLogic playerLogic;
        private readonly IMessageWriter writer;
        private readonly IPlayerKnowledge playerKnowledge;
        private readonly PlayerOptions playerOptions;

        private bool gameFinished = false;

        public DataMessageHandler(
            IMessageWriter writer,
            IPlayerLogic playerLogic,
            IPlayerKnowledge playerKnowledge,
            PlayerOptions playerOptions)
        {
            this.playerLogic = playerLogic;
            this.writer = writer;
            this.playerKnowledge = playerKnowledge;
            this.playerOptions = playerOptions;
        }

        public override void Handle(DataMessage message)
        {
            if (gameFinished)
            {
                logger.Debug("Received message after game finished");
                return;
            }

            var messagePlayerLocation = message.PlayerLocation;
            var messageGoalFields = message.GoalFields;
            var messagePieces = message.Pieces;
            var messageGameFinished = message.GameFinished;
            var messageTaskFields = message.TaskFields;

            var board = playerKnowledge.GameState.Board;

            if (messageGameFinished)
            {
                // todo: display game state
                writer.Write(new GetGamesMessage(), playerOptions.RetryJoinGameInterval);
                gameFinished = true;
                return;
            }

            if (messagePlayerLocation != null)
            {
                board.MovePlayer(playerKnowledge.Player, new Position(messagePlayerLocation.X, messagePlayerLocation.Y));
            }
            messageGoalFields?.ForEach(UpdateGoalField);
            messageTaskFields?.ForEach(UpdateTaskField);
            messagePieces?.ForEach(UpdatePiece);

            //if (knowledge.Player.Team == TeamColor.Red) return;

            IMessage response = playerLogic.GetNextMove();
            writer.Write(response);
        }

        private void UpdateGoalField(GoalField field)
        {
            var board = playerKnowledge.GameState.Board;

            var tile = board.Fields[field.X, field.Y] as GoalTile;
            tile.Timestamp = field.Timestamp;
            if (field.PlayerIdSpecified)
            {
                var player = playerKnowledge.GameState.Players.Find(p => p.Id == field.PlayerId);
                board.MovePlayer(player, new Position(field.X, field.Y));
            }
            if (field.Type != GoalFieldType.Unknown) tile.Type = field.Type;
        }

        private void UpdateTaskField(TaskField field)
        {
            var board = playerKnowledge.GameState.Board;

            var tile = board.Fields[field.X, field.Y] as TaskTile;
            tile.Timestamp = field.Timestamp;
            if (field.PlayerIdSpecified)
            {
                var player = playerKnowledge.GameState.Players.Find(p => p.Id == field.PlayerId);
                board.MovePlayer(player, new Position(field.X, field.Y));
            }
            if (field.PieceIdSpecified)
            {
                var boardPiece = board.Pieces.Find(p => p.Id == field.PieceId);
                if (boardPiece == null)
                {
                    boardPiece = new BoardPiece(field.PieceId, null, PieceType.Unknown, new Position(field.X, field.Y));
                    board.Pieces.Add(boardPiece);
                }

                var foundTile = board.Fields.OfType<TaskTile>().ToList().Find(t => t.Piece == boardPiece);
                if (foundTile != null)
                {
                    foundTile.Piece = null;
                }

                tile.Piece = boardPiece;
            }
            else
            {
                tile.Piece = null;
            }
            tile.DistanceToPiece = field.DistanceToPiece;
        }

        private void UpdatePiece(Piece piece)
        {
            // find the board piece equivalent (must exist because it must have been discovered first)
            var board = playerKnowledge.GameState.Board;

            var boardPiece = board.Pieces.Find(p => p.Id == piece.Id);
            // if its a sham forget about it
            if (piece.Type == PieceType.Sham)
            {
                return;
            }
            // if we know its type set it
            if (piece.Type != PieceType.Unknown)
            {
                boardPiece.Type = piece.Type;
            }
            // if players is carrying it remember that
            if (piece.PlayerIdSpecified)
            {
                GamePlayer player = playerKnowledge.GameState.Players.Find(p => p.Id == piece.PlayerId);
                boardPiece.SetPlayer(player);
                if (piece.PlayerId == playerKnowledge.Player.Id)
                {
                    playerKnowledge.SetCarriedPiece(boardPiece);
                }
            }
        }
    }
}