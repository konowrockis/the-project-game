cd src\TheProjectGame.CommunicationServer\bin\Debug
start TheProjectGame.CommunicationServer.exe --port 20000 -v
cd ..\..\..\..\

cd src\TheProjectGame.GameMaster\bin\Debug
start TheProjectGame.GameMaster.exe --port 20000 -a localhost -v --GameOptions.NumberOfPlayersPerTeam 1
cd ..\..\..\..\

cd src\TheProjectGame.Player\bin\Debug
start TheProjectGame.Player.exe --port 20000 --address "127.0.0.1" -v
cd ..\..\..\..\

cd src\TheProjectGame.Player\bin\Debug
start TheProjectGame.Player.exe --port 20000 --address "127.0.0.1" -v
cd ..\..\..\..\