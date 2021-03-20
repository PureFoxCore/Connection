using System;
using System.Net;
using CSLogger;
using Connection;

namespace TestClient
{
    public class Program
    {
        private static ClientTCP client = new ClientTCP();

        static void Main(string[] args)
        {
            Logger.Trace("Starting Client...");

            client.Connect(IPAddress.Parse("127.0.0.1"), 8192);

            Console.ReadKey();
            client.CloseConnection();
        }
    }
}
