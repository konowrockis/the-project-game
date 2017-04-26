using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(ElementName = "TestPiece")]
    public class TestPieceMessage : GameMessage, IMessage
    { }
}
