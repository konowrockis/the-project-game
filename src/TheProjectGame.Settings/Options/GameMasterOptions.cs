using System.Xml.Serialization;

namespace TheProjectGame.Settings.Options
{
    [XmlRoot(ElementName = "GameMasterSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class GameMasterOptions : NetworkOptions
    {
        private const int DefaultRetryRegisterGameInterval = 5000;

        public GameOptions GameDefinition { get; set; }

        public ActionCostsOptions ActionCosts { get; set; }

        [XmlAttribute("retryRegisterGameInterval")]
        public uint RetryRegisterGameInterval { get; set; } = DefaultRetryRegisterGameInterval;
    }
}
