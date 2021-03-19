using System;
using CSLogger;
using Connection;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Trace("Starting Server...");

            TcpServer.Start(8192, 20);

            Console.ReadKey();
        }
    }
}
