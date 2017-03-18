using System.IO;
using System.Runtime.Serialization;
using TheProjectGame.Settings.FromFile.Interfaces;

namespace TheProjectGame.Settings.FromFile.Classes
{
    public class ParametersSerializer<T> : IParametersSerializer<T>
        where T : class
    {
        public void Serialize(T parameter, string filename)
        {               
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (var stream = new FileStream(filename,FileMode.Create,FileAccess.Write))
            {
                serializer.WriteObject(stream,parameter);
            }
        }
    }
}
