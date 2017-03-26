using System.Xml.Serialization;

namespace TheProjectGame.Settings.Options
{
    [XmlRoot(ElementName = "CommunicationServerSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class CommunicationServerOptions : NetworkOptions
    {
    }
}
