namespace TheProjectGame.Settings.FromFile.Interfaces
{
    interface IParametersDeserializer<T>
        where T: class
    {
       T Deserialize(string filename);
    }
}
