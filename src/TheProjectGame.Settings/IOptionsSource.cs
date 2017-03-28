using System.IO;

namespace TheProjectGame.Settings
{
    public interface IOptionsSource
    {
        Stream GetConfiguration();
    }
}
