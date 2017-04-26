using System.Xml.Serialization;
using TheProjectGame.Contracts.Messages.Structures;

namespace TheProjectGame.Contracts.Messages.PlayerActions
{
    [XmlRoot(ElementName = "PickUpPiece")]
    public class PickUpPieceMessage : GameMessage, IMessage
    { }
}
