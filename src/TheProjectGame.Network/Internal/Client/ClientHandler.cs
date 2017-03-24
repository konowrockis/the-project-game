using System;
using System.IO;
using System.Linq;
using System.Net;
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
    public class ByteStream : NetworkStream
    {
        private const byte ETB = 0x17;

        public ByteStream(Socket s) : base(s)
        { }

        public override int Read(byte[] buffer, int offset, int size)
        {
            int read = base.Read(buffer, offset, size);

            if (read == 1 && buffer[0] == ETB)
            {
                return Read(buffer, offset, size);
            }

            int move = 0;

            for (int i = 0; i < read; i++)
            {
                if (buffer[i] == ETB)
                {
                    move++;
                }
                else if (move != 0)
                {
                    buffer[i - move] = buffer[i];
                }
            }

            return read - move;
        }

        public override int ReadByte()
        {
            int b = base.ReadByte();
            return b == ETB ? ReadByte() : b;
        }
    }

    internal class ClientHandler : INetworkHandler
    {
        private readonly IClientSocket socket;
        
        private readonly IConnection connection;
        private readonly ILifetimeScope lifetimeScope;
        private readonly Action setup;

        public delegate ClientHandler Factory(IClientSocket socket);

        public ClientHandler(IClientSocket socket, IOptions<NetworkOptions> networkOptions, ILifetimeScope lifetimeScope)
        {
            this.socket = socket;
            this.connection = new Connection(socket);
            this.lifetimeScope = lifetimeScope;

            setup = () =>
            {
                if (!socket.Connected)
                { 
                    var endPoint = new IPEndPoint(IPAddress.Parse(networkOptions.Value.Address), networkOptions.Value.Port);
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
