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

            server.Start(8192, 20);

            Console.ReadKey();
        }
    }
}
