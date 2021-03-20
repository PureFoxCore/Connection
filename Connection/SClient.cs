using System;
using CSLogger;
using System.Net.Sockets;

namespace Connection
{
    public class SClient
    {
        public int ConnectionID;
        public string ip;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] readBuffer;

        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            readBuffer = new byte[4096];
            stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, ReciveDataCallback, null);
        }

        private void ReciveDataCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = stream.EndRead(result);
                if (readBytes <= 0)
                {
                    CloseConnection();
                    return;
                }
                byte[] newBytes = new byte[readBytes];
                Buffer.BlockCopy(readBuffer, 0, newBytes, 0, readBytes);
                stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, ReciveDataCallback, null);
            }
            catch (Exception ex)
            {
                Logger.Critical($"Error while handling data: {ex}");
                throw;
            }
        }

        private void CloseConnection()
        {
            Logger.Info($"Connection from {ip} terminated.");
            socket.Close();
        }
    }
}