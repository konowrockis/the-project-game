using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
