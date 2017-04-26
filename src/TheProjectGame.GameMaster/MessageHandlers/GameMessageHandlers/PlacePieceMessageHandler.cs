using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.Logging;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class PlacePieceMessageHandler : MessageHandler<PlacePiece>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;
        private readonly GameMasterOptions options;

        public PlacePieceMessageHandler(IMessageWriter messageWriter, ActionCostsOptions actionCosts, ICurrentGame currentGame, GameMasterOptions options)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = actionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.options = options;
        }

        public override void Handle(PlacePiece message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);

            var builder = new DataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            var piece = board.Pieces.FirstOrDefault(p => p.Player == player);

            if (!board.IsInGoalArea(player.Position) && piece!=null)
            {
                board.Pieces.Remove(piece);
                board.PlaceNewPiece();
                messageWriter.Write(builder.Build(), actionCosts.PlacingDelay);
                /*board.Pieces.Add(piece);
                bool result = board.DropPiece(piece, player.Position);
                if (result)
                {
                    builder.Fields(board.Fields[player.Position.X, player.Position.Y]);
                }
                else
                {
                    builder.Pieces(false,piece);
                }
                board.RefreshBoardState();
                messageWriter.Write(builder.Build(), actionCosts.PlacingDelay);
                return;*/
            }

            if (piece != null)
            {
                board.Pieces.Remove(piece);
                board.PlaceNewPiece();
            }

            if (piece == null || piece.Type == PieceType.Sham)
            {
                messageWriter.Write(builder.Build(), actionCosts.PlacingDelay);
                return;
            }
            
            var goalTile = board.Fields[player.Position.X, player.Position.Y] as GoalTile;
            goalTile.Discovered = true;

            if (board.CheckWinConditions(player.Team))
            {
                foreach (var gamePlayer in game.Players)
                {
                    logger.GameEvent(gamePlayer.Team == player.Team
                        ? GameEvent.CreateVictory(message.PlayerGuid, game.Id, player.Id, player.Team, player.Role)
                        : GameEvent.CreateDefeat(message.PlayerGuid, game.Id, player.Id, player.Team, player.Role));

                    messageWriter.Write(new Data()
                    {
                        GameFinished = true,
                        PlayerId = gamePlayer.Id
                    });
                }
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
            }
            else
            {
                var response = builder.Fields(goalTile).Build();
                messageWriter.Write(response, actionCosts.PlacingDelay);
            }
        }
    }
}
