using System;
using CSLogger;
using System.Net;
using System.Net.Sockets;

namespace Connection
{
    public class ServerTCP
    {
        public SClient[] Clients;
        private TcpListener serverSocket;

        public void Start(int port, int maxPlayers)
        {
            Clients = new SClient[maxPlayers];
            for (int i = 0; i < Clients.Length; i++)
                Clients[i] = new SClient();
            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            Logger.Info($"Server started on: {serverSocket.LocalEndpoint}");
            serverSocket.BeginAcceptTcpClient(ClientConnectCallback, null);
        }

        private void ClientConnectCallback(IAsyncResult result)
        {
            TcpClient tempClient = serverSocket.EndAcceptTcpClient(result);
            Logger.Info("Player Succesfully connected");
            serverSocket.BeginAcceptTcpClient(ClientConnectCallback, null);

            for (int i = 0; i < Clients.Length; i++)
                if (Clients[i].socket == null)
                {
                    Clients[i].socket = tempClient;
                    Clients[i].ConnectionID = i;
                    Clients[i].ip = tempClient.Client.RemoteEndPoint.ToString();
                    Clients[i].Start();
                    Logger.Info($"Connection recived from: {Clients[i].ip}.");
                    return;
                }
        }
    }
}