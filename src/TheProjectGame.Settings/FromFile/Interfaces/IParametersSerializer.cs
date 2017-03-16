namespace TheProjectGame.Settings.FromFile.Interfaces
{
    interface IParametersSerializer<T>
        where T: class
    {
        void Serialize(T parameter, string filename);
    }
}
