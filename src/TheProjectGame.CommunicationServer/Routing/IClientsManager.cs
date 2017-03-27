namespace TheProjectGame.CommunicationServer.Routing
{
    public interface IClientsManager
    {
        void Add(IClient client);
        void Remove(IClient client);

        ulong GetNewPlayerId();

        IClient GetPlayerById(ulong id);
    }
}
