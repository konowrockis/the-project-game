using System.Collections.Generic;
using Serilog;
using TheProjectGame.Game;
using TheProjectGame.Messaging;
using TheProjectGame.Player.Game;

namespace TheProjectGame.Player.MessageHandlers
{
    class GameMessageHandler : MessageHandler<Contracts.Messages.GameActions.GameMessage>
    {
        private ILogger logger = Log.ForContext<GameMessageHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly IPlayerLogic playerLogic;
        private readonly IPlayerKnowledge playerKnowledge;

        public GameMessageHandler(
            IMessageWriter messageWriter, 
            IPlayerKnowledge playerKnowledge, 
            IPlayerLogic playerLogic)
        {
            this.messageWriter = messageWriter;
            this.playerKnowledge = playerKnowledge;
            this.playerLogic = playerLogic;
        }

        public override void Handle(Contracts.Messages.GameActions.GameMessage message)
        {
            var position = new Position(message.PlayerLocation.X, message.PlayerLocation.Y);
            playerKnowledge.Player.Position = position;
            var allPlayers = message.Players;
            List<GamePlayer> players = new List<GamePlayer>();

            var board = new Board(message.Board.Width, message.Board.TasksHeight, message.Board.GoalsHeight, 0);

            // set distances to -1
            foreach (var field in board.Fields)
            {
                if (field is TaskTile)
                {
                    var taskTile = field as TaskTile;
                    taskTile.DistanceToPiece = -1;
                }
            }
            // add players to list and place on board
            foreach (var player in allPlayers)
            {
                if (player.Id == playerKnowledge.Player.Id)
                {
                    players.Add(playerKnowledge.Player);
                    continue;
                }
                GamePlayer gamePlayer = new GamePlayer(player.Id);
                gamePlayer.Role = player.Type;
                gamePlayer.Team = player.Team;
                gamePlayer.Position = null;
                players.Add(gamePlayer);
            }

            board.Fields[message.PlayerLocation.X, message.PlayerLocation.Y].Player = playerKnowledge.Player;

            playerKnowledge.GameState.Players = players;
            playerKnowledge.GameState.Board = board;

            var response = playerLogic.GetNextMove();
            messageWriter.Write(response);
        }
    }
}
