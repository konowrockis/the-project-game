using System.Xml.Serialization;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public class TaskField : Field
    {
        [XmlAttribute("distanceToPiece")]
        public uint DistanceToPiece { get; set; }

        [XmlAttribute("pieceId")]
        public ulong PieceId { get; set; }

        [XmlIgnore]
        public bool PieceIdSpecified { get; set; }
    }
}
