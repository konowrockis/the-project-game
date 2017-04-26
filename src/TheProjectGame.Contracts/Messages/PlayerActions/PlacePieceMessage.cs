using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(ElementName = "PlacePiece")]
    public class PlacePieceMessage : GameMessage, IMessage
    { }
}
