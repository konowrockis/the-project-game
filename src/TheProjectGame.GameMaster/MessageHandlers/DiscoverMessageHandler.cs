using System;
using System.Collections.Generic;
using System.Linq;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class DiscoverMessageHandler : MessageHandler<Discover>
    {
        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly GameState game;
        private readonly PlayersMap players;

        public DiscoverMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, GameState game, PlayersMap players)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = game;
            this.players = players;
        }

        public override void Handle(Discover message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);

            if (player == null) return;

            List<GoalTile> goals = new List<GoalTile>();
            List<TaskTile> tasks = new List<TaskTile>();
            List<BoardPiece> pieces = new List<BoardPiece>();

            foreach (var tile in board.GetNeighbourhood(player.Position.X, player.Position.Y))
            {
                if (tile is TaskTile)
                {
                    var taskTile = tile as TaskTile;

                    tasks.Add(taskTile);

                    if (taskTile.Piece != null)
                    {
                        pieces.Add(taskTile.Piece);
                    }
                }
                else if (tile is GoalTile)
                {
                    goals.Add(tile as GoalTile);
                }
            }
            
            DateTime timestamp = DateTime.Now;

            var response = new Data()
            {
                TaskFields = tasks.Select(t => new TaskField()
                {
                    DistanceToPiece = t.DistanceToPiece,
                    PieceId = t.Piece?.Id ?? 0,
                    PieceIdSpecified = t.Piece != null,
                    Timestamp = timestamp,
                    PlayerId = t.Player?.Id ?? 0,
                    PlayerIdSpecified = t.Player != null,
                    X = t.X,
                    Y = t.Y
                }).ToList(),
                GoalFields = goals.Select(t => new GoalField()
                {
                    PlayerId = t.Player?.Id ?? 0,
                    PlayerIdSpecified = t.Player != null,
                    X = t.X,
                    Y = t.Y,
                    Timestamp = timestamp,
                    Team = t.Team,
                    Type = t.Type
                }).ToList(),
                GameFinished = false,
                PlayerId = player.Id,
                PlayerLocation = player.Position.ToLocation(),
                Pieces = pieces.Select(p => new Piece()
                {
                    Id = p.Id,
                    Timestamp = timestamp,
                    PlayerId = p.Player?.Id ?? 0,
                    PlayerIdSpecified = p.Player != null,
                    Type = p.Type
                }).ToList()
            };

            messageWriter.Write(response, actionCosts.DiscoverDelay);
        }
    }
}
