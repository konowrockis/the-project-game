using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Autofac;
using TheProjectGame.Network.Internal;
using TheProjectGame.Network.Internal.Contract;
using TheProjectGame.Network.Internal.Exceptions;
using TheProjectGame.Settings;
using TheProjectGame.Settings.Options;

namespace TheProjectGame.Network
{
    internal class ClientHandler : INetworkHandler
    {
        private readonly IClientSocket socket;
        
        private readonly IConnection connection;
        private readonly ILifetimeScope lifetimeScope;
        private readonly Action setup;

        public delegate ClientHandler Factory(IClientSocket socket);

        public ClientHandler(IClientSocket socket, GeneralOptions networkOptions, ILifetimeScope lifetimeScope)
        {
            this.socket = socket;
            this.connection = new Connection(socket);
            this.lifetimeScope = lifetimeScope;

            setup = () =>
            {
                if (!socket.Connected)
                {
                    IPHostEntry entry = Dns.GetHostEntry(networkOptions.Address);
                    IPAddress[] addresses = entry.AddressList;
                    IPAddress addr = addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                    if (addr == null) throw new InvalidAddressException();
                    var endPoint = new IPEndPoint(addr, networkOptions.Port);
                    socket.Connect(endPoint);
                }
            };
        }

        public void Run()
        {
            using (var scope = lifetimeScope.BeginLifetimeScope())
            {
                IClientEventHandler eventHandler = scope.Resolve<IClientEventHandler>();

                bool opened = false;
                try
                {
                    setup();
                    opened = true;

                    eventHandler.OnOpen(connection, new ByteStream(socket.RawSocket));
                }
                catch (SocketClosedException)
                {
                    socket.Close();
                }
                catch (ObjectDisposedException)
                {
                    // ignored
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode != 10054 && e.ErrorCode != 10053)
                    {
                        IConnectionData connData = connection;
                        eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
                    }
                }
                catch (IOException)
                {
                    // ignored
                }
                catch (Exception e)
                {
                    IConnectionData connData = connection;
                    eventHandler.OnError(opened ? connData : new VoidConnectionData(), e);
                }
                finally
                {
                    if (opened) eventHandler.OnClose(connection);
                }
            }
        }
    }
}
