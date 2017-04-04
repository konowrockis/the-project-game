using System.IO;

namespace TheProjectGame.Messaging
{
    class SchemaSource : ISchemaSource
    {
        private const string SchemaFileName = "TheProjectGameCommunication.xsd";
        public Stream GetSchema()
        {
            return File.OpenRead(SchemaFileName);
        }
    }
}
