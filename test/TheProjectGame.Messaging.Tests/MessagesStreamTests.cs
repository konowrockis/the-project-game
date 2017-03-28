using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;

namespace TheProjectGame.Messaging.Tests
{
    [TestClass]
    public class MessagesStreamTests
    {
        [TestMethod]
        public void Parse_GetGames_Message()
        {
            var stream = getMessageStream("GetGames.xml");

            var message = stream.Read() as GetGames;

            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void Parse_RegisterGame_Message()
        {
            var stream = getMessageStream("RegisterGame.xml");

            var message = stream.Read() as RegisterGame;

            Assert.IsNotNull(message);
            Assert.AreEqual("easyGame", message.NewGameInfo.Name);
            Assert.AreEqual<ulong>(2, message.NewGameInfo.BlueTeamPlayers);
            Assert.AreEqual<ulong>(2, message.NewGameInfo.RedTeamPlayers);
        }

        [TestMethod]
        public void Parse_ConfirmGameRegistration_Message()
        {
            var stream = getMessageStream("ConfirmGameRegistration.xml");

            var message = stream.Read() as ConfirmGameRegistration;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
        }

        [TestMethod]
        public void Parse_RegisteredGames_Message()
        {
            var stream = getMessageStream("RegisteredGames.xml");

            var message = stream.Read() as RegisteredGames;

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
            var stream = getMessageStream("JoinGame.xml");

            var message = stream.Read() as JoinGame;

            Assert.IsNotNull(message);
            Assert.AreEqual("easyGame", message.GameName);
            Assert.AreEqual(PlayerType.Leader, message.PreferedRole);
            Assert.AreEqual(TeamColor.Blue, message.PreferedTeam);
        }

        [TestMethod]
        public void Parse_ConfirmJoiningGame_Message()
        {
            var stream = getMessageStream("ConfirmJoingingGame.xml");

            var message = stream.Read() as ConfirmJoiningGame;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PrivateGuid);
            Assert.AreEqual<ulong>(2, message.PlayerDefinition.Id);
            Assert.AreEqual(TeamColor.Blue, message.PlayerDefinition.Team);
            Assert.AreEqual(PlayerType.Player, message.PlayerDefinition.Type);
        }

        [TestMethod]
        public void Parse_Game_Message()
        {
            var stream = getMessageStream("Game.xml");

            var message = stream.Read() as Game;

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
            var stream = getMessageStream("Discover.xml");

            var message = stream.Read() as Discover;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_Discover_Response_Message()
        {
            var stream = getMessageStream("DiscoverResponse.xml");

            var message = stream.Read() as Data;

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
            var stream = getMessageStream("Move.xml");

            var message = stream.Read() as Move;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
            Assert.AreEqual(MoveType.Up, message.Direction);
        }

        [TestMethod]
        public void Parse_Move_Proper_Response_Message()
        {
            var parser = getMessageStream("MoveProperResponse.xml");

            var message = parser.Read() as Data;

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
            var stream = getMessageStream("MoveOccupiedResponse.xml");

            var message = stream.Read() as Data;

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
            var stream = getMessageStream("PickUpPiece.xml");

            var message = stream.Read() as PickUpPiece;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_PickUpPiece_Response_Message()
        {
            var stream = getMessageStream("PickUpPieceResponse.xml");

            var message = stream.Read() as Data;

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
            var stream = getMessageStream("TestPiece.xml");

            var message = stream.Read() as TestPiece;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_AuthorizeKnowledgeExchange_Message()
        {
            var stream = getMessageStream("AuthorizeKnowledgeExchange.xml");

            var message = stream.Read() as AuthorizeKnowledgeExchange;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(1, message.GameId);
            Assert.AreEqual<ulong>(2, message.WithPlayerId);
            Assert.AreEqual("c094cab7-da7b-457f-89e5-a5c51756035f", message.PlayerGuid);
        }

        [TestMethod]
        public void Parse_KnowledgeExchangeRequest_Message()
        {
            var stream = getMessageStream("KnowledgeExchangeRequest.xml");

            var message = stream.Read() as KnowledgeExchangeRequest;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual<ulong>(1, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_RejectKnowledgeExchange_Message()
        {
            var stream = getMessageStream("RejectKnowledgeExchange.xml");

            var message = stream.Read() as RejectKnowledgeExchange;

            Assert.IsNotNull(message);
            Assert.IsFalse(message.Permanent);
            Assert.AreEqual<ulong>(1, message.PlayerId);
            Assert.AreEqual<ulong>(2, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_AcceptExchangeRequest_Message()
        {
            var stream = getMessageStream("AcceptExchangeRequest.xml");

            var message = stream.Read() as AcceptExchangeRequest;

            Assert.IsNotNull(message);
            Assert.AreEqual<ulong>(2, message.PlayerId);
            Assert.AreEqual<ulong>(2, message.SenderPlayerId);
        }

        [TestMethod]
        public void Parse_KnowledgeExchange_Response_Message()
        {
            var stream = getMessageStream("KnowledgeExchangeResponse.xml");

            var message = stream.Read() as Data;

            Assert.IsNotNull(message);
            Assert.AreEqual(2, message.TaskFields.Count);
            Assert.AreEqual<uint>(1, message.TaskFields[0].X);
            Assert.AreEqual<uint>(5, message.TaskFields[0].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 11) - message.TaskFields[0].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(5, message.TaskFields[0].DistanceToPiece);
            Assert.AreEqual(2, message.GoalFields.Count);
            Assert.AreEqual<uint>(0, message.GoalFields[0].X);
            Assert.AreEqual<uint>(9, message.GoalFields[0].Y);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 17) - message.GoalFields[0].Timestamp).TotalSeconds);
            Assert.AreEqual<uint>(5, message.TaskFields[0].DistanceToPiece);
            Assert.AreEqual(TeamColor.Blue, message.GoalFields[0].Team);
            Assert.AreEqual(GoalFieldType.Goal, message.GoalFields[1].Type);
            Assert.AreEqual<ulong>(2, message.GoalFields[1].PlayerId);
            Assert.IsTrue(message.GoalFields[1].PlayerIdSpecified);
            Assert.AreEqual(2, message.Pieces.Count);
            Assert.AreEqual<ulong>(1, message.Pieces[0].Id);
            Assert.AreEqual(0, (new DateTime(2017, 2, 23, 17, 20, 09) - message.Pieces[0].Timestamp).TotalSeconds);
            Assert.AreEqual(PieceType.Sham, message.Pieces[0].Type);
            Assert.IsFalse(message.Pieces[0].PlayerIdSpecified);
        }

        private MessageStream getMessageStream(string messageName)
        {
            var messageContent = getMessageFromResource(messageName);

            var messageSource = Substitute.For<ISchemaSource>();
            messageSource.GetSchema().Returns(getSchemaStream());

            return new MessageStream(messageContent, new MessagesParser(), messageSource);
        }

        private Stream getMessageFromResource(string message)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceStream = assembly.GetManifestResourceStream("TheProjectGame.Messaging.Tests.TestMessages." + message);

            MemoryStream ms = new MemoryStream();
            resourceStream.CopyTo(ms);
            ms.WriteByte(0x17);
            ms.Position = 0;

            return ms;
        }

        private Stream getSchemaStream()
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream("TheProjectGame.Messaging.Tests.TheProjectGameCommunication.xsd");
        }
    }
}
