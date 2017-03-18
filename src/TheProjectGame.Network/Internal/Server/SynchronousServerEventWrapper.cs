using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual void OnError(IConnectionData data, Exception exception)
        {
            lock (mutex)
            {
                serverEventHandler.OnError(data, exception);
            }
        }

        public virtual void OnMessage(IConnection connection, string message)
        {
            lock (mutex)
            {
                serverEventHandler.OnMessage(connection, message);
            }
        }

        public virtual void OnOpen(IConnection connection)
        {
            lock (mutex)
            {
                serverEventHandler.OnOpen(connection);
            }
        }

        public virtual void OnServerError(Exception exception)
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
