using System;
using System.Net;
using CSLogger;
using Connection;

namespace TestClient
{
    public class Program
    {
        private static ClientTCP client = new ClientTCP();

        private static void Main(string[] args)
        {
            Logger.Trace("Starting Client...");

            client.DataRecived += DataRecived;            
            client.Connect(IPAddress.Parse("127.0.0.1"), 8192);

            string t;
            do
            {
                t = Console.ReadLine();
                if (t != "" || t != "quit")
                {
                    ByteBuffer data = new ByteBuffer();
                    data.WriteString(t);
                    client.SendData(data);
                }
            } while (t != "quit");
            client.CloseConnection();
        }

        private static void DataRecived(ByteBuffer data)
        {
            Logger.Info(data.ReadString());
        }
    }
}
