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

        public DefaultMessagesParser()
        {
            messageSerializers = new Dictionary<string, XmlSerializer>();

            initMessageSerializers();
        }

        private void initMessageSerializers()
        {
            addMessageSerializer<GetGames>();
            addMessageSerializer<RegisterGame>();
            addMessageSerializer<ConfirmGameRegistration>();
            addMessageSerializer<RegisteredGames>();
            addMessageSerializer<JoinGame>();
            addMessageSerializer<ConfirmJoiningGame>();
            addMessageSerializer<Game>();
            addMessageSerializer<GameFinished>();
            addMessageSerializer<RejectGameRegistration>();

            addMessageSerializer<Discover>();
            addMessageSerializer<Move>();
            addMessageSerializer<PickUpPiece>();
            addMessageSerializer<PlacePiece>();
            addMessageSerializer<TestPiece>();

            addMessageSerializer<Data>();

            addMessageSerializer<KnowledgeExchangeRequest>();
            addMessageSerializer<AcceptExchangeRequest>();
            addMessageSerializer<AuthorizeKnowledgeExchange>();
            addMessageSerializer<RejectKnowledgeExchange>();
        }

        private void addMessageSerializer<T>() where T: IMessage
        {
            var t = typeof(T);
            messageSerializers[t.Name] = new XmlSerializer(t, DefaultNamespace);
        }

        public IMessage Parse(string messageName, XmlReader reader)
        {
            return messageSerializers[messageName].Deserialize(reader) as IMessage;
        }

        public void Write(Stream stream, IMessage message)
        {
            var t = message.GetType();
            MemoryStream buffer = new MemoryStream();
            messageSerializers[t.Name].Serialize(buffer, message);
            buffer.WriteByte(ETB);
            var bytes = buffer.ToArray();
            stream.Write(bytes,0,bytes.Length);
        }
    }
}
