using System;
using CSLogger;
using Connection;

namespace TestServer
{
    public class Program
    {
        private static ServerTCP server = new ServerTCP();
        
        static void Main(string[] args)
        {
            Logger.Trace("Starting Server...");

            server.DataRecived += OnDataRecived;
            server.ClientConnected += OnClientConnected;
            server.ClientDisconnected += OnClientDisconnected;
            server.Start(8192, 20);

            string t;
            do
            {
                t = Console.ReadLine();
                if (t != "" || t != "quit")
                {
                    ByteBuffer data = new ByteBuffer();
                    data.WriteString($"Server: {t}");
                    server.SendDataToAll(data);
                }
            } while (t != "quit");
        }

        private static void OnDataRecived(int id, ByteBuffer data)
        {
            ByteBuffer tData = new ByteBuffer();
            string message = data.ReadString();
            tData.WriteString($"User: {id}; Message: {message}");
            server.SendDataToAllExept(id, tData);
        }

        private static void OnClientConnected(SClient client)
        {
            ByteBuffer data = new ByteBuffer();
            data.WriteString($"Server: User with id {client.ConnectionID} connected.");
            server.SendDataToAllExept(client.ConnectionID, data);            
        }

        private static void OnClientDisconnected(SClient client)
        {
            ByteBuffer data = new ByteBuffer();
            data.WriteString($"Server: User with id {client.ConnectionID} disconnected.");
            server.SendDataToAll(data);
        }
    }
}
