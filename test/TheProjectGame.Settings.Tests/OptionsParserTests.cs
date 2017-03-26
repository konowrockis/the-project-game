using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheProjectGame.Settings.Options;
using TheProjectGame.Settings.Options.Structures;

namespace TheProjectGame.Settings.Tests
{
    [TestClass]
    public class OptionsParserTests
    {
        [TestMethod]
        public void No_player_config_reads_default_values()
        {
            var parser = GetOptionsParser(@"no-config.xml");

            var options = parser.GetOptions<PlayerOptions>();

            Assert.AreEqual(30000, options.KeepAliveInterval);
            Assert.AreEqual<uint>(5000, options.RetryJoinGameInterval);
        }

        [TestMethod]
        public void No_game_master_config_reads_default_values()
        {
            var parser = GetOptionsParser(@"no-config.xml");

            var options = parser.GetOptions<GameMasterOptions>();

            Assert.AreEqual(30000, options.KeepAliveInterval);
            Assert.AreEqual<uint>(5000, options.RetryRegisterGameInterval);
        }

        [TestMethod]
        public void No_communication_server_config_reads_default_values()
        {
            var parser = GetOptionsParser(@"no-config.xml");

            var options = parser.GetOptions<CommunicationServerOptions>();

            Assert.AreEqual(30000, options.KeepAliveInterval);
        }

        [TestMethod]
        public void No_config_reads_game_default_values()
        {
            var parser = GetOptionsParser(@"no-config.xml");

            var options = parser.GetOptions<GameMasterOptions>().GameDefinition;

            Assert.IsNotNull(options.Goals);
            Assert.AreEqual(0.1, options.ShamProbability);
            Assert.AreEqual<uint>(1000, options.PlacingNewPiecesFrequency);
            Assert.AreEqual<uint>(4, options.InitialNumberOfPieces);
            Assert.AreEqual<uint>(5, options.BoardWidth);
            Assert.AreEqual<uint>(7, options.TaskAreaLength);
            Assert.AreEqual<uint>(3, options.GoalAreaLength);
            Assert.AreEqual<uint>(4, options.NumberOfPlayersPerTeam);
        }

        [TestMethod]
        public void No_config_reads_action_costs_default_values()
        {
            var parser = GetOptionsParser(@"no-config.xml");

            var options = parser.GetOptions<GameMasterOptions>().ActionCosts;

            Assert.AreEqual<uint>(100, options.MoveDelay);
            Assert.AreEqual<uint>(450, options.DiscoverDelay);
            Assert.AreEqual<uint>(500, options.TestDelay);
            Assert.AreEqual<uint>(100, options.PickUpDelay);
            Assert.AreEqual<uint>(100, options.PlacingDelay);
            Assert.AreEqual<uint>(1200, options.KnowledgeExchangeDelay);
        }

        [TestMethod]
        public void Parsing_player_configuration_reads_values_properly()
        {
            var parser = GetOptionsParser(@"Configurations\PlayerConfiguration.xml");

            var options = parser.GetOptions<PlayerOptions>();

            Assert.AreEqual(9999, options.KeepAliveInterval);
            Assert.AreEqual<uint>(8888, options.RetryJoinGameInterval);
        }

        [TestMethod]
        public void Parsing_game_master_configuration_reads_values_properly()
        {
            var parser = GetOptionsParser(@"Configurations\GameMasterConfiguration.xml");

            var options = parser.GetOptions<GameMasterOptions>();

            Assert.AreEqual(9999, options.KeepAliveInterval);
            Assert.AreEqual<uint>(8888, options.RetryRegisterGameInterval);
        }

        [TestMethod]
        public void Parsing_communication_server_configuration_reads_values_properly()
        {
            var parser = GetOptionsParser(@"Configurations\CommunicationServerConfiguration.xml");

            var options = parser.GetOptions<CommunicationServerOptions>();

            Assert.AreEqual(9999, options.KeepAliveInterval);
        }

        [TestMethod]
        public void Parsing_game_configuration_reads_values_properly()
        {
            var parser = GetOptionsParser(@"Configurations\GameMasterConfiguration.xml");

            var options = parser.GetOptions<GameMasterOptions>().GameDefinition;

            Assert.AreEqual(2, options.Goals.Count);
            Assert.AreEqual(TeamColor.Red, options.Goals[0].Team);
            Assert.AreEqual(GoalFieldType.Goal, options.Goals[0].Type);
            Assert.AreEqual<uint>(0, options.Goals[0].X);
            Assert.AreEqual<uint>(5, options.Goals[0].Y);
            Assert.AreEqual(0.33, options.ShamProbability);
            Assert.AreEqual<uint>(2500, options.PlacingNewPiecesFrequency);
            Assert.AreEqual<uint>(1, options.InitialNumberOfPieces);
            Assert.AreEqual<uint>(2, options.BoardWidth);
            Assert.AreEqual<uint>(4, options.TaskAreaLength);
            Assert.AreEqual<uint>(1, options.GoalAreaLength);
            Assert.AreEqual<uint>(3, options.NumberOfPlayersPerTeam);
            Assert.AreEqual("Initial game", options.GameName);
        }

        [TestMethod]
        public void Parsing_action_costs_configuration_reads_values_properly()
        {
            var parser = GetOptionsParser(@"Configurations\GameMasterConfiguration.xml");

            var options = parser.GetOptions<GameMasterOptions>().ActionCosts;

            Assert.AreEqual<uint>(101, options.MoveDelay);
            Assert.AreEqual<uint>(451, options.DiscoverDelay);
            Assert.AreEqual<uint>(501, options.TestDelay);
            Assert.AreEqual<uint>(101, options.PickUpDelay);
            Assert.AreEqual<uint>(101, options.PlacingDelay);
            Assert.AreEqual<uint>(1501, options.KnowledgeExchangeDelay);
        }

        [TestMethod]
        public void Parsing_command_line_network_options_works_properly()
        {
            var parser = GetOptionsParser(@"no-config.xml", "--address", "\"99.88.77.66\"", "--port", "9999");

            var options = parser.GetOptions<PlayerOptions>();

            Assert.AreEqual("\"99.88.77.66\"", options.Address);
            Assert.AreEqual(9999, options.Port);
        }

        [TestMethod]
        public void Overriding_options_with_command_line_works_properly()
        {
            var parser = GetOptionsParser(@"Configurations\GameMasterConfiguration.xml",
                "--GameMasterOptions.RetryRegisterGameInterval", "1111",
                "--ActionCostsOptions.MoveDelay", "2222",
                "--GameOptions.InitialNumberOfPieces", "3333");

            var options = parser.GetOptions<GameMasterOptions>();

            Assert.AreEqual<uint>(1111, options.RetryRegisterGameInterval);
            Assert.AreEqual<uint>(2222, options.ActionCosts.MoveDelay);
            Assert.AreEqual<uint>(3333, options.GameDefinition.InitialNumberOfPieces);
        }

        private OptionsParser GetOptionsParser(string configLocation, params string[] additionalParams)
        {
            var parameters = new string[] { "abc.exe", "--conf", configLocation };

            return new OptionsParser(parameters.Union(additionalParams).ToArray());
        }
    }
}
