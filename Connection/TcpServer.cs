using System;
using System.Net;
using System.Net.Sockets;

namespace Connection
{
    public class TcpServer
    {
        private static TcpListener serverSocket;

        public static void Start(int port)
        {
            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(ClientConnectCallBack, null);
        }

        private static void ClientConnectCallBack(IAsyncResult result)
        {
            
        }
    }
}