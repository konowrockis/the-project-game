using System.Xml.Serialization;
using CommandLine;

namespace TheProjectGame.Settings.Options
{
    [XmlRoot(ElementName = "GameMasterSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class GameMasterOptions : GeneralOptions
    {
        private const int DefaultRetryRegisterGameInterval = 5000;

        public GameOptions GameDefinition { get; set; } = new GameOptions();

        public ActionCostsOptions ActionCosts { get; set; } = new ActionCostsOptions();

        [XmlAttribute("RetryRegisterGameInterval")]
        [Option(nameof(GameMasterOptions) + "." + nameof(RetryRegisterGameInterval))]
        public uint RetryRegisterGameInterval { get; set; } = DefaultRetryRegisterGameInterval;
    }
}
