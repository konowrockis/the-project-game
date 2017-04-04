using TheProjectGame.Network.Client;

namespace TheProjectGame.Network.Server
{
    interface IServerSocket
    {
        void Listen(int port);
        IClientSocket Accept();
    }
}
