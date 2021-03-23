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

        public delegate void DataHandler(SClient client, ByteBuffer data);
        public event DataHandler DataRecived;
        public delegate void ClientDisconnectedHandler(SClient client);
        public event ClientDisconnectedHandler ClientDisconnected;

        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            readBuffer = new byte[4096];
            stream.BeginRead(readBuffer, 0, 4096, ReciveDataCallback, null);
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

                if (DataRecived != null)
                    DataRecived(this, new ByteBuffer(newBytes));
                
                stream.BeginRead(readBuffer, 0, 4096, ReciveDataCallback, null);
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

            if (ClientDisconnected != null)
                    ClientDisconnected(this);
        }
    }
}