namespace TheProjectGame.Network
{
    public interface IConnection : IConnectionData
    {
        void Close();
    }
}
