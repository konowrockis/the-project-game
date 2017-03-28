using System.IO;

namespace TheProjectGame.Messaging
{
    public interface ISchemaSource
    {
        Stream GetSchema();
    }
}
