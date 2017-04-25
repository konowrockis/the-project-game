﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TheProjectGame.CommunicationServer.MessageHandlers;
using TheProjectGame.CommunicationServer.Routing;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.GameActions;

namespace TheProjectGame.CommunicationServer.Tests
{
    [TestClass]
    public class GetGamesMessageHandlerTests
    {
        private const string gameName = "testName";

        private IClient player;
        private IGamesManager gamesManager;
        private GetGamesMessageHandler messageHandler;

        public GetGamesMessageHandlerTests()
        {
            player = Substitute.For<IClient>();
            gamesManager = Substitute.For<IGamesManager>();

            var currentClient = Substitute.For<ICurrentClient>();

            currentClient.Value.Returns(player);
            gamesManager.GetGamesList().Returns(new List<IGame>());

            messageHandler = new GetGamesMessageHandler(currentClient, gamesManager);
        }

        [TestMethod]
        public void Send_response_to_client()
        {
            var message = GetMessage();

            messageHandler.Handle(message);

            player.Received().Write(Arg.Any<RegisteredGames>());
        }

        [TestMethod]
        public void Get_empty_list_when_no_games_registerd()
        {
            var message = GetMessage();

            messageHandler.Handle(message);

            player.Received().Write(Arg.Is<RegisteredGames>(m => 
                m.GameInfo != null &&
                m.GameInfo.Count == 0
            ));
        }

        [TestMethod]
        public void Get_games_list_when_one_game_is_registerd()
        {
            SetGamesList();
            var message = GetMessage();

            messageHandler.Handle(message);

            Guid ignoreGuid;
            player.Received().Write(Arg.Is<RegisteredGames>(m =>
                m.GameInfo != null &&
                m.GameInfo.Count == 1 &&
                m.GameInfo[0].BlueTeamPlayers == TestGame.playersPerTeam &&
                m.GameInfo[0].RedTeamPlayers == TestGame.playersPerTeam &&
                Guid.TryParse(m.GameInfo[0].Name, out ignoreGuid)
            ));
        }

        [TestMethod]
        public void Get_games_list_when_multiple_games_are_registered()
        {
            const int gamesCount = 10;
            SetGamesList(gamesCount);
            var message = GetMessage();

            messageHandler.Handle(message);

            player.Received().Write(Arg.Is<RegisteredGames>(m =>
                m.GameInfo != null &&
                m.GameInfo.Count == gamesCount
            ));
        }

        private GetGames GetMessage()
        {
            return new GetGames();
        }

        private void SetGamesList(int count = 1)
        {
            gamesManager.GetGamesList().Returns(
                Enumerable.Range(0, count)
                .Select(i => new TestGame())
                .ToList()
            );
        }

        private class TestGame : IGame
        {
            private static ulong id = 1;
            public const int playersPerTeam = 5;

            public ulong Id { get; }
            public string Name { get; }
            public ulong BlueTeamPlayers => playersPerTeam;
            public ulong RedTeamPlayers => playersPerTeam;

            public IClient GameMaster
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            
            public TestGame()
            {
                Id = id++;
                Name = Guid.NewGuid().ToString();
            }
        }
    }
}
