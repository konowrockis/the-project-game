using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheProjectGame.IntegrationTests
{
    [TestClass]
    [TestCategory("Integration")]
    public class GameMasterTests
    {
        private int portBase = 40000;
        private const string communicationServerAddress = "127.0.0.1";
        private const string gameMasterLocation = @"..\..\..\..\src\TheProjectGame.GameMaster\bin\Debug";
        private const string gameMasterName = "TheProjectGame.GameMaster.exe";

        [TestMethod]
        [Timeout(5000)]
        public void Connect_with_GameMaster()
        {
            int port = portBase + 1;
            var clientProcess = RunGameMaster(port);
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
        public void Wait_for_GameMaster_message()
        {
            int port = portBase + 2;
            var clientProcess = RunGameMaster(port);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            byte[] buffer = new byte[10240];

            socket.Bind(endPoint);
            socket.Listen(10);
            var client = socket.Accept();
            client.Receive(buffer);

            Assert.IsTrue(client.Connected);
            Assert.IsTrue(Encoding.UTF8.GetString(buffer).Contains("RegisterGame"));
            clientProcess.Kill();
        }

        private Process RunGameMaster(int port)
        {
            Process playerProcess = new Process();

            playerProcess.StartInfo.FileName = gameMasterName;
            playerProcess.StartInfo.Arguments = $"--port {port} --address {communicationServerAddress}";
            playerProcess.StartInfo.WorkingDirectory = gameMasterLocation;
            playerProcess.Start();

            return playerProcess;
        }
    }
}
