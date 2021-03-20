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

        public delegate void OnClientHandler(SClient client);
        public event OnClientHandler ClientConnected;

        public delegate void DataHandler(int id, ByteBuffer data);
        public event DataHandler DataRecived;

        public delegate void ClientDisconnectedHandler(SClient client);
        public event ClientDisconnectedHandler ClientDisconnected;

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
                    Clients[i].DataRecived += OnData;
                    Clients[i].ClientDisconnected += OnClientDisconnected;
                    if (ClientConnected != null)
                        ClientConnected(Clients[i]);
                    Logger.Info($"Connection recived. IP: {Clients[i].ip}, ID: {i}.");
                    return;
                }
        }

        private void OnClientDisconnected(SClient client)
        {
            if (ClientDisconnected != null)
                    ClientDisconnected(client);
        }

        private void OnData(int id, ByteBuffer data)
        {
            if (DataRecived != null)
                DataRecived(id, data);
        }

        public void SendDataTo(int id, ByteBuffer data)
        {
            Clients[id].stream.BeginWrite(data.ToArray, 0, data.ToArray.Length, null, null);
        }

        public void SendDataToAll(ByteBuffer data)
        {
            for (int i = 0; i < Clients.Length; i++)
            try
            {
                if (Clients[i].socket != null)
                    Clients[i].stream.BeginWrite(data.ToArray, 0, data.ToArray.Length, null, null);
            }
            catch (Exception ex)
            {
                Logger.Error($"Can't send data to client with ID [{i}], {ex}");
                throw;
            }
        }

        public void SendDataToAllExept(int id, ByteBuffer data)
        {
            for (int i = 0; i < Clients.Length; i++)
            try
            {
                if (Clients[i].socket != null && Clients[i].ConnectionID != id)
                    Clients[i].stream.BeginWrite(data.ToArray, 0, data.ToArray.Length, null, null);
            }
            catch (Exception ex)
            {
                Logger.Error($"Can't send data to client with ID [{i}], {ex}");
                throw;
            }
        }
    }
}