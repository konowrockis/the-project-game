﻿using System;
using AutoMapper;
using Serilog;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Game;
using TheProjectGame.Game.Builders;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.MessageHandlers
{
    class MoveMessageHandler : MessageHandler<MoveMessage>
    {
        private readonly ILogger logger = Log.ForContext<GameMasterEventHandler>();

        private readonly IMessageWriter messageWriter;
        private readonly ActionCostsOptions actionCosts;
        private readonly IGameState game;
        private readonly IPlayersMap players;
        private readonly ICurrentGame currentGame;
        private readonly Func<DataBuilder> dataBuilder;

        public MoveMessageHandler(
            IMessageWriter messageWriter, 
            GameMasterOptions gameMasterOptions, 
            ICurrentGame currentGame,
            Func<DataBuilder> dataBuilder)
        {
            this.messageWriter = messageWriter;
            this.actionCosts = gameMasterOptions.ActionCosts;
            this.game = currentGame.Game;
            this.players = currentGame.Players;
            this.dataBuilder = dataBuilder;
            this.currentGame = currentGame;
        }

        public override void Handle(MoveMessage message)
        {
            var board = game.Board;
            var player = players.GetPlayer(message.PlayerGuid);
            var direction = message.Direction;
            var position = player.Position;
            var destination = position.Move(direction);
            var moveStatus = CheckMove(destination, board);

            var builder = dataBuilder()
                .GameFinished(false)
                .PlayerId(player.Id);

            if (moveStatus == MoveStatus.Valid)
            {
                board.MovePlayer(player, destination);
            }
            board.RefreshBoardState();
            builder.PlayerLocation(player.Position);
            if (moveStatus != MoveStatus.Invalid)
            {
                builder.Fields(board.Fields[destination.X, destination.Y]);
            }

            var response = builder.Build();
            messageWriter.Write(response, actionCosts.MoveDelay);

            currentGame.UpdateGame();
        }

        private MoveStatus CheckMove(Position destination, IBoard board)
        {
            if (!board.IsValid(destination)) return MoveStatus.Invalid;
            if (board.IsOccupied(destination)) return MoveStatus.Occupied;
            return MoveStatus.Valid;
        }

        private enum MoveStatus
        {
            Valid,
            Occupied,
            Invalid
        }
    }
}