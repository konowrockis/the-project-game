using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;

namespace TheProjectGame.Messaging.Default
{
    class DefaultMessagesParser : IMessageParser
    {
        private const string DefaultNamespace = "http://theprojectgame.mini.pw.edu.pl/";
        private const byte ETB = 0x17;

        private readonly Dictionary<string, XmlSerializer> messageSerializers;
        private readonly Dictionary<Type, XmlSerializer> messageSerializersByType;

        public DefaultMessagesParser()
        {
            messageSerializers = new Dictionary<string, XmlSerializer>();
            messageSerializersByType = new Dictionary<Type, XmlSerializer>();

            initMessageSerializers();
        }

        private void initMessageSerializers()
        {
            addMessageSerializer<GetGamesMessage>("GetGames");
            addMessageSerializer<RegisterGameMessage>("RegisterGame");
            addMessageSerializer<ConfirmGameRegistrationMessage>("ConfirmGameRegistration");
            addMessageSerializer<RegisteredGamesMessage>("RegisteredGames");
            addMessageSerializer<JoinGameMessage>("JoinGame");
            addMessageSerializer<ConfirmJoiningGameMessage>("ConfirmJoiningGame");
            addMessageSerializer<GameStartedMessage>("Game");
            addMessageSerializer<GameFinished>("GameFinished");
            addMessageSerializer<RejectGameRegistrationMessage>("RejectGameRegistration");
            addMessageSerializer<RejectJoiningGameMessage>("RejectJoiningGame");

            addMessageSerializer<DiscoverMessage>("Discover");
            addMessageSerializer<MoveMessage>("Move");
            addMessageSerializer<PickUpPieceMessage>("PickUpPiece");
            addMessageSerializer<PlacePieceMessage>("PlacePiece");
            addMessageSerializer<TestPieceMessage>("TestPiece");

            addMessageSerializer<DataMessage>("Data");

            addMessageSerializer<KnowledgeExchangeRequestMessage>("KnowledgeExchangeRequest");
            addMessageSerializer<AcceptExchangeRequestMessage>("AcceptExchangeRequest");
            addMessageSerializer<AuthorizeKnowledgeExchangeMessage>("AuthorizeKnowledgeExchange");
            addMessageSerializer<RejectKnowledgeExchangeMessage>("RejectKnowledgeExchange");
        }

        private void addMessageSerializer<T>(string name) where T : IMessage
        {
            var t = typeof(T);
            var serializer = new XmlSerializer(t, DefaultNamespace);
            messageSerializers[name] = serializer;
            messageSerializersByType[t] = serializer;
        }

        public IMessage Parse(string messageName, XmlReader reader)
        {
            return messageSerializers[messageName].Deserialize(reader) as IMessage;
        }

        public void Write(Stream stream, IMessage message)
        {
            var t = message.GetType();
            MemoryStream buffer = new MemoryStream();
            messageSerializersByType[t].Serialize(buffer, message);
            buffer.WriteByte(ETB);
            var bytes = buffer.ToArray();
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
