using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;
using TheProjectGame.Messaging;

namespace TheProjectGame.Messaging.Tests
{
    [TestClass]
    public class MessagesParserTests
    {
        [TestMethod]
        public void Parse_GetGames_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("GetGames.xml");

            var message = parser.Parse(messageContent) as GetGames;

            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void Parse_RegisterGame_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("RegisterGame.xml");

            var message = parser.Parse(messageContent) as RegisterGame;

            Assert.IsNotNull(message);
            Assert.AreEqual("easyGame", message.NewGameInfo.Name);
            Assert.AreEqual<ulong>(2, message.NewGameInfo.BlueTeamPlayers);
            Assert.AreEqual<ulong>(2, message.NewGameInfo.RedTeamPlayers);
        }

        [TestMethod]
        public void Parse_ConfirmGameRegistration_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("ConfirmGameRegistration.xml");

            var message = parser.Parse(messageContent) as ConfirmGameRegistration;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
        }

        [TestMethod]
        public void Parse_RegisteredGames_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("RegisteredGames.xml");

            var message = parser.Parse(messageContent) as RegisteredGames;

            Assert.IsNotNull(message);
            Assert.AreEqual(2, message.GameInfo.Count);
            Assert.AreEqual("easyGame", message.GameInfo[0].Name);
            Assert.AreEqual<ulong>(2, message.GameInfo[0].BlueTeamPlayers);
            Assert.AreEqual<ulong>(2, message.GameInfo[0].RedTeamPlayers);
            Assert.AreEqual("hardForBlueGame", message.GameInfo[1].Name);
            Assert.AreEqual<ulong>(5, message.GameInfo[1].BlueTeamPlayers);
            Assert.AreEqual<ulong>(10, message.GameInfo[1].RedTeamPlayers);
        }

        [TestMethod]
        public void Parse_JoinGame_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("JoinGame.xml");

            var message = parser.Parse(messageContent) as JoinGame;

            Assert.IsNotNull(message);
            Assert.AreEqual("easyGame", message.GameName);
            Assert.AreEqual(PlayerType.Leader, message.PreferedRole);
            Assert.AreEqual(TeamColour.Blue, message.PreferedTeam);
        }

        [TestMethod]
        public void Parse_ConfirmJoiningGame_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("ConfirmJoingingGame.xml");

            var message = parser.Parse(messageContent) as ConfirmJoiningGame;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PrivateGuid);
            Assert.AreEqual<ulong>(2, message.PlayerDefinition.Id);
            Assert.AreEqual(TeamColour.Blue, message.PlayerDefinition.Team);
            Assert.AreEqual(PlayerType.Player, message.PlayerDefinition.Type);
        }

        [TestMethod]
        public void Parse_Game_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("Game.xml");

            var message = parser.Parse(messageContent) as Game;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual(8, message.Players.Count);
            Assert.AreEqual<uint>(5, message.Board.Width);
            Assert.AreEqual<uint>(5, message.Board.TasksHeight);
            Assert.AreEqual<uint>(3, message.Board.GoalsHeight);
            Assert.AreEqual<uint>(0, message.PlayerLocation.X);
            Assert.AreEqual<uint>(3, message.PlayerLocation.Y);
        }

        [TestMethod]
        public void Parse_Discover_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("Discover.xml");

            var message = parser.Parse(messageContent) as Discover;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_Discover_Response_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("DiscoverResponse.xml");

            var message = parser.Parse(messageContent) as Data;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.IsFalse(message.GameFinished);
            Assert.AreEqual(9, message.TaskFields.Count);
            Assert.AreEqual<uint>(1, message.TaskFields[0].X);
            Assert.AreEqual<uint>(4, message.TaskFields[0].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.TaskFields[0].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(1, message.TaskFields[0].DistanceToPiece);
            Assert.IsFalse(message.TaskFields[0].PieceIdSpecified);
            Assert.IsFalse(message.TaskFields[0].PlayerIdSpecified);
            Assert.AreEqual<uint>(1, message.TaskFields[1].X);
            Assert.AreEqual<uint>(5, message.TaskFields[1].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.TaskFields[1].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(0, message.TaskFields[1].DistanceToPiece);
            Assert.IsTrue(message.TaskFields[1].PieceIdSpecified);
            Assert.IsTrue(message.TaskFields[1].PlayerIdSpecified);
            Assert.AreEqual<ulong>(2, message.TaskFields[1].PieceId);
            Assert.AreEqual<ulong>(2, message.TaskFields[1].PlayerId);
            Assert.IsNull(message.PlayerLocation);
            Assert.AreEqual(0, message.Pieces.Count);
            Assert.AreEqual(0, message.GoalFields.Count);
        }

        [TestMethod]
        public void Parse_Move_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("Move.xml");

            var message = parser.Parse(messageContent) as Move;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
            Assert.AreEqual(MoveType.Up, message.Direction);
        }

        [TestMethod]
        public void Parse_Move_Proper_Response_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("MoveProperResponse.xml");

            var message = parser.Parse(messageContent) as Data;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.IsFalse(message.GameFinished);
            Assert.AreEqual(1, message.TaskFields.Count);
            Assert.AreEqual<uint>(1, message.TaskFields[0].X);
            Assert.AreEqual<uint>(5, message.TaskFields[0].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.TaskFields[0].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(5, message.TaskFields[0].DistanceToPiece);
            Assert.IsFalse(message.TaskFields[0].PieceIdSpecified);
            Assert.IsFalse(message.TaskFields[0].PlayerIdSpecified);
            Assert.AreEqual<uint>(1, message.PlayerLocation.X);
            Assert.AreEqual<uint>(5, message.PlayerLocation.Y);
            Assert.AreEqual(0, message.Pieces.Count);
            Assert.AreEqual(0, message.GoalFields.Count);
        }

        [TestMethod]
        public void Parse_Move_Occupied_Response_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("MoveOccupiedResponse.xml");

            var message = parser.Parse(messageContent) as Data;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.IsFalse(message.GameFinished);
            Assert.AreEqual(1, message.TaskFields.Count);
            Assert.AreEqual<uint>(1, message.TaskFields[0].X);
            Assert.AreEqual<uint>(5, message.TaskFields[0].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.TaskFields[0].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(0, message.TaskFields[0].DistanceToPiece);
            Assert.IsTrue(message.TaskFields[0].PieceIdSpecified);
            Assert.IsTrue(message.TaskFields[0].PlayerIdSpecified);
            Assert.AreEqual<ulong>(2, message.TaskFields[0].PlayerId);
            Assert.AreEqual<ulong>(2, message.TaskFields[0].PieceId);
            Assert.AreEqual<uint>(1, message.PlayerLocation.X);
            Assert.AreEqual<uint>(4, message.PlayerLocation.Y);
            Assert.AreEqual(1, message.Pieces.Count);
            Assert.AreEqual<ulong>(2, message.Pieces[0].Id);
            Assert.AreEqual<ulong>(2, message.Pieces[0].PlayerId);
            Assert.AreEqual(PieceType.Unknown, message.Pieces[0].Type);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.Pieces[0].Timestamp).TotalSeconds);
            Assert.AreEqual(0, message.GoalFields.Count);
        }

        [TestMethod]
        public void Parse_PickUpPiece_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("PickUpPiece.xml");

            var message = parser.Parse(messageContent) as PickUpPiece;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_PickUpPiece_Response_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("PickUpPieceResponse.xml");

            var message = parser.Parse(messageContent) as Data;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.IsFalse(message.GameFinished);
            Assert.AreEqual(1, message.Pieces.Count);
            Assert.AreEqual<ulong>(2, message.Pieces[0].Id);
            Assert.AreEqual<ulong>(1, message.Pieces[0].PlayerId);
            Assert.AreEqual(PieceType.Unknown, message.Pieces[0].Type);
            Assert.AreEqual(0, (new DateTime(2017, 2, 27, 12, 00, 34) - message.Pieces[0].Timestamp).TotalSeconds);
            Assert.AreEqual(0, message.TaskFields.Count);
            Assert.AreEqual(0, message.GoalFields.Count);
            Assert.IsNull(message.PlayerLocation);
        }

        [TestMethod]
        public void Parse_TestPiece_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("TestPiece.xml");

            var message = parser.Parse(messageContent) as TestPiece;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_AuthorizeKnowledgeExchange_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("AuthorizeKnowledgeExchange.xml");

            var message = parser.Parse(messageContent) as AuthorizeKnowledgeExchange;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual<ulong>(2, message.WithPlayerId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_KnowledgeExchangeRequest_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("KnowledgeExchangeRequest.xml");

            var message = parser.Parse(messageContent) as KnowledgeExchangeRequest;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual<ulong>(1, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_RejectKnowledgeExchange_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("RejectKnowledgeExchange.xml");

            var message = parser.Parse(messageContent) as RejectKnowledgeExchange;

            Assert.IsNotNull(message);
            Assert.IsFalse(message.Permanent);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.AreEqual<ulong>(2, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_AcceptExchangeRequest_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("AcceptExchangeRequest.xml");

            var message = parser.Parse(messageContent) as AcceptExchangeRequest;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual<ulong>(2, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_KnowledgeExchange_Response_Message()
        {
            var parser = getMessagesParser();
            var messageContent = getMessageFromResource("KnowledgeExchangeResponse.xml");

            var message = parser.Parse(messageContent) as Data;

            Assert.IsNotNull(message);
            // TODO: write asserts
        }

        private MessagesParser getMessagesParser()
        {
            return new MessagesParser();
        }

        private Stream getMessageFromResource(string message)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream("TheProjectGame.Messaging.Tests.TestMessages." + message);
        }
    }
}
