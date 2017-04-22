using System.IO;

namespace TheProjectGame.Messaging.Default
{
    class DefaultSchemaSource : ISchemaSource
    {
        private const string SchemaFileName = "TheProjectGameCommunication.xsd";
        public Stream GetSchema()
        {
            return File.OpenRead(SchemaFileName);
        }
    }
}
