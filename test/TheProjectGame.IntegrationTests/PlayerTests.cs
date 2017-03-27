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
    public class PlayerTests
    {
        private int portBase = 30000;
        private const string communicationServerAddress = "127.0.0.1";
        private const string playerLocation = @"..\..\..\..\src\TheProjectGame.Player\bin\Debug";
        private const string playerName = "TheProjectGame.Player.exe";

        [TestMethod]
        [Timeout(5000)]
        public void Connect_with_Player()
        {
            int port = portBase + 1;
            var clientProcess = RunClient(port);
            var endPoint = new IPEndPoint(IPAddress.Any, port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(10);
            var client = socket.Accept();

            Assert.IsTrue(client.Connected);
            clientProcess.Kill();
        }

        [TestMethod]
        [Timeout(5000)]
        public void Wait_for_Player_message()
        {
            int port = portBase + 2;
            var clientProcess = RunClient(port);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] buffer = new byte[10240];

            socket.Bind(endPoint);
            socket.Listen(10);
            var client = socket.Accept();
            client.Receive(buffer);

            Assert.IsTrue(client.Connected);
            Assert.IsTrue(Encoding.UTF8.GetString(buffer).Contains("GetGames"));
            clientProcess.Kill();
        }

        private Process RunClient(int port)
        {
            Process playerProcess = new Process();

            playerProcess.StartInfo.FileName = playerName;
            playerProcess.StartInfo.Arguments = $"--port {port} --address {communicationServerAddress}";
            playerProcess.StartInfo.WorkingDirectory = playerLocation;
            playerProcess.Start();

            return playerProcess;
        }
    }
}
