using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class DataMessageHandler : MessageHandler<Data>
    {
        private IBoard board;
        private IPlayerLogic playerLogic;
        private IMessageWriter writer;
        private PlayerKnowledge knowledge;

        private bool gameFinished = false;


        public DataMessageHandler(IMessageWriter writer, IPlayerLogic playerLogic, PlayerKnowledge playerKnowledge)
        {
            this.board = playerKnowledge.GameState.Board;
            this.playerLogic = playerLogic;
            this.writer = writer;
            this.knowledge = playerKnowledge;
        }

        public override void Handle(Data message)
        {
            if (gameFinished)
            {
                return;
            }
            var messagePlayerLocation = message.PlayerLocation;
            var messageGoalFields = message.GoalFields;
            var messagePieces = message.Pieces;
            var messageGameFinished = message.GameFinished;
            var messageTaskFields = message.TaskFields;

            if (messageGameFinished)
            {
                // todo: display game state
                gameFinished = true;
                return;
            }

            if (messagePlayerLocation != null)
            {
                board.MovePlayer(knowledge.Player, new Position(messagePlayerLocation.X, messagePlayerLocation.Y));
            }
            messageGoalFields?.ForEach(UpdateGoalField);
            messageTaskFields?.ForEach(UpdateTaskField);
            messagePieces?.ForEach(UpdatePiece);

            //if (knowledge.Player.Team == TeamColor.Red) return;

            IMessage response = playerLogic.GetNextMove(board, knowledge);
            writer.Write(response);
        }

        private void UpdateGoalField(GoalField field)
        {
            var tile = board.Fields[field.X, field.Y] as GoalTile;
            tile.Timestamp = field.Timestamp;
            if (field.PlayerIdSpecified)
            {
                var player = knowledge.GameState.Players.Find(p => p.Id == field.PlayerId);
                board.MovePlayer(player,new Position(field.X,field.Y));
            }
            if (field.Type != GoalFieldType.Unknown) tile.Type = field.Type;
        }

        private void UpdateTaskField(TaskField field)
        {
            var tile = board.Fields[field.X, field.Y] as TaskTile;
            tile.Timestamp = field.Timestamp;
            if (field.PlayerIdSpecified)
            {
                var player = knowledge.GameState.Players.Find(p => p.Id == field.PlayerId);
                board.MovePlayer(player, new Position(field.X, field.Y));
            }
            if (field.PieceIdSpecified)
            {
                var boardPiece = board.Pieces.Find(p => p.Id == field.PieceId);
                if (boardPiece == null)
                {
                    boardPiece = new BoardPiece(field.PieceId,null,PieceType.Unknown, new Position(field.X,field.Y));
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
            var boardPiece = board.Pieces.Find(p => p.Id == piece.Id);
            if (piece.Type != PieceType.Unknown)
            {
                boardPiece.Type = piece.Type;
            }
            if (piece.PlayerIdSpecified)
            {
                GamePlayer player = knowledge.GameState.Players.Find(p => p.Id == piece.PlayerId);
                boardPiece.SetPlayer(player);
                if (piece.PlayerId == knowledge.Player.Id)
                {
                    knowledge.CarriedPiece = boardPiece;
                }
            }

        }
    }
}
