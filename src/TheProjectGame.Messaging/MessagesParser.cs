using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TheProjectGame.Contracts;
using TheProjectGame.Contracts.Messages.CommunicationActions;
using TheProjectGame.Contracts.Messages.GameActions;
using TheProjectGame.Contracts.Messages.PlayerActions;

namespace TheProjectGame.Messaging
{
    public class MessagesParser : IMessageParser
    {
        private readonly Dictionary<string, XmlSerializer> messageSerializers;

        public MessagesParser()
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

            messageSerializers[t.Name] = new XmlSerializer(t);
        }

        public IMessage Parse(string messageName, XmlReader reader)
        {
            return messageSerializers[messageName].Deserialize(reader) as IMessage;
        }

        public void Write(Stream stream, IMessage message)
        {
            var t = message.GetType();

            messageSerializers[t.Name].Serialize(stream, message);
        }
    }
}
