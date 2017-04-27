using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheProjectGame.IntegrationTests
{
    [TestClass]
    [TestCategory("Integration")]
    public class CommunicationServerTests
    {
        private Process communicationServerProcess;

        private int port = 20000;
        private const string communicationServerAddress = "127.0.0.1";
        private const string communicationServerLocation = @"..\..\..\..\src\TheProjectGame.CommunicationServer\bin\Debug";
        private const string communicationServerName = "TheProjectGame.CommunicationServer.exe";

        [TestInitialize]
        public void TestInitialize()
        {
            communicationServerProcess = new Process();

            communicationServerProcess.StartInfo.FileName = communicationServerName;
            communicationServerProcess.StartInfo.Arguments = $"--port {port}";
            communicationServerProcess.StartInfo.WorkingDirectory = communicationServerLocation;
            communicationServerProcess.Start();
        }

        [TestMethod]
        [Timeout(5000)]
        public void Connect_to_CommunicationServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(communicationServerAddress), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endPoint);

            Assert.IsTrue(socket.Connected);
        }

        [TestMethod]
        [Timeout(5000)]
        public void Get_response_for_GetGames_message()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(communicationServerAddress), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] buffer = new byte[10240];

            socket.Connect(endPoint);
            socket.Send(GetMessage());
            socket.Receive(buffer);

            Assert.IsTrue(socket.Connected);
            Assert.IsTrue(Encoding.UTF8.GetString(buffer).Contains("RegisteredGames"));
        }

        [TestMethod]
        [Timeout(5000)]
        public void Reconnect_with_CommunicationServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(communicationServerAddress), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket reconnectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(endPoint);
            socket.Dispose();
            reconnectSocket.Connect(endPoint);

            Assert.IsTrue(reconnectSocket.Connected);
        }

        [TestMethod]
        [Timeout(20000)]
        public void Stress_test_CommunicationServer()
        {
            const int numberOfClients = 100;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(communicationServerAddress), port);
            List<Socket> sockets = Enumerable.Range(0, numberOfClients)
                .Select(i => new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)).ToList();

            sockets.ForEach(socket => socket.Connect(endPoint));

            sockets.TrueForAll(socket => socket.Connected);
        }

        private byte[] GetMessage()
        {
            return Encoding.UTF8
                .GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\" ?><GetGames xmlns=\"http://theprojectgame.mini.pw.edu.pl/\" />")
                .Concat(new List<byte> { 0x17 })
                .ToArray();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            communicationServerProcess.Kill();
        }
    }
}
