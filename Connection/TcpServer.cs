using System;
using CSLogger;
using System.Net;
using System.Net.Sockets;

namespace Connection
{
    public class TcpServer
    {
        public static SClient[] Clients;
        private static TcpListener serverSocket;

        public static void Start(int port, int maxPlayers)
        {
            Clients = new SClient[maxPlayers];
            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            Logger.Info($"Server started on: {serverSocket.LocalEndpoint}");
            serverSocket.BeginAcceptTcpClient(ClientConnectCallBack, null);
        }

        private static void ClientConnectCallBack(IAsyncResult result)
        {
            TcpClient tempClient = serverSocket.EndAcceptTcpClient(result);
            Logger.Info("Player Succesfully connected");
            serverSocket.BeginAcceptTcpClient(ClientConnectCallBack, null);

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