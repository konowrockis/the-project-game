using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;
using TheProjectGame.GameMaster.Games;
using TheProjectGame.GameMaster.MessageHandlers;
using TheProjectGame.Messaging;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.GameMaster.Tests
{
    [TestClass]
    public class MoveMessageHandlerTests
    {
        private const ulong GameId = 1;
        private const double MoveDelay = 100;

        [TestMethod]
        public void Send_proper_response_to_valid_move_message()
        {
            var systemUnderTests = TestSetup();
            GamePlayer player = CreatePlayer(1, 0, 0);
            Location destination = new Location(0, 1);
            string guid = systemUnderTests.PlayersMap.AddPlayer(player);
            var message = CreateMessage(MoveType.Down, GameId, guid);


            systemUnderTests.Handler.Handle(message);
            /*
            Data expected = new Data.Builder().GameFinished(false).PlayerId(player.Id).PlayerLocation(destination).GoalFields(new GoalField()
            {
                PlayerId = player.Id,
                PlayerIdSpecified = true,
                Team = player.Team,
                Type = GoalFieldType.Unknown,
                X=0,
                Y=1
            }).Build();
            Data response = null;
            double delay = 0;
            systemUnderTests.Writer.When(w=>w.Write(Arg.Any<Data>(),Arg.Any<double>())).Do(call =>
            {
                response = call.Arg<Data>();
                delay = call.Arg<double>();
                expected.GoalFields.First().Timestamp = response.GoalFields.First().Timestamp;
            });

            
           
            Assert.IsTrue(delay.Equals(MoveDelay));
            Assert.IsTrue(expected.Equals(response));*/
        }


        private Move CreateMessage(MoveType direction, ulong gameId, string playerGuid)
        {
            return new Move()
            {
                Direction = direction,
                GameId = gameId,
                PlayerGuid = playerGuid
            };
        }

        private SystemUnderTests TestSetup()
        {
            IMessageWriter writer = Substitute.For<IMessageWriter>();
            GameState state = new GameState(GameId, 10, 10, 10);
            var playersMap = new PlayersMap();
            MoveMessageHandler handler = new MoveMessageHandler(writer, new ActionCostsOptions(),state,playersMap);
            return new SystemUnderTests(handler,writer,state,playersMap);
        }

        private GamePlayer CreatePlayer(ulong id, uint x, uint y)
        {
            return new GamePlayer(id)
            {
                Position = new Position(x,y)
            };
        }

        private class SystemUnderTests
        {
            public MoveMessageHandler Handler { get;  }
            public IMessageWriter Writer { get;  }
            public GameState GameState { get; }
            public PlayersMap PlayersMap { get; }

            public SystemUnderTests(MoveMessageHandler handler, IMessageWriter writer, GameState gameState, PlayersMap playersMap)
            {
                Handler = handler;
                Writer = writer;
                GameState = gameState;
                PlayersMap = playersMap;
            }
        }



    }
}
