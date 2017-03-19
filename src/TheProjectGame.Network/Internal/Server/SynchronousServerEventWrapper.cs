using System.IO;

namespace TheProjectGame.Network.Internal.Server
{
    internal abstract class SynchronousServerEventWrapper : IServerEventHandler
    {
        protected readonly IServerEventHandler serverEventHandler;
        protected readonly object mutex;

        protected SynchronousServerEventWrapper(IServerEventHandler serverEventHandler)
        {
            this.serverEventHandler = serverEventHandler;
            this.mutex = new object();
        }

        public virtual void OnClose(IConnectionData data)
        {
            lock (mutex)
            {
                serverEventHandler.OnClose(data);
            }
        }

        public virtual void OnError(IConnectionData data, System.Exception exception)
        {
            lock (mutex)
            {
                serverEventHandler.OnError(data, exception);
            }
        }

        public virtual void OnOpen(IConnection connection, Stream stream)
        {
            lock (mutex)
            {
                serverEventHandler.OnOpen(connection, stream);
            }
        }

        public virtual void OnServerError(System.Exception exception)
        {
            lock (mutex)
            {
                serverEventHandler.OnServerError(exception);
            }
        }

        public virtual void OnServerStart()
        {
            lock (mutex)
            {
                serverEventHandler.OnServerStart();
            }
        }

        public virtual void OnServerStop()
        {
            lock (mutex)
            {
                serverEventHandler.OnServerStop();
            }
        }
    }
}
