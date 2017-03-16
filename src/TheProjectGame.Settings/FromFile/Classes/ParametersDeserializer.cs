using System;
using System.IO;
using System.Runtime.Serialization;
using TheProjectGame.Settings.FromFile.Interfaces;

namespace TheProjectGame.Settings.FromFile.Classes
{
    public class ParametersDeserializer<T> : IParametersDeserializer<T> where T : class
    {
        public T Deserialize(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Config file does not exist.");
                return null;
            }
            if (!filename.EndsWith(".xml"))
            {
                Console.WriteLine("Not a xml file.");
                return null;
            }
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
