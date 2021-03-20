using System;
using CSLogger;
using System.Net;
using System.Net.Sockets;

namespace Connection
{
    public class ClientTCP
    {
        public TcpClient client;
        public NetworkStream stream;
        private byte[] asyncBuffer;
        public bool isConnected;

        public delegate void DataHandler(ByteBuffer data);
        public event DataHandler DataRecived;

        public void Connect(IPAddress address, int port)
        {
            Logger.Trace($"Attempting to connect [{address}:{port}]");
            client = new TcpClient();
            client.ReceiveBufferSize = 4096;
            client.SendBufferSize = 4096;
            asyncBuffer = new byte[8192];
            try
            {
                client.BeginConnect(address, port, new AsyncCallback(ConnectCallback), client);
            }
            catch (Exception ex)
            {
                Logger.Error($"Unable to connect to the server {ex}");
                throw;
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                client.EndConnect(result);
                if (!client.Connected)
                    return;
                stream = client.GetStream();
                stream.BeginRead(asyncBuffer, 0, 8192, OnReciveDataCallback, null);
                isConnected = true;
                Logger.Info($"Connected to server.");
            }
            catch (Exception ex)
            {
                Logger.Critical($"Can't connect to server: {ex}");
                isConnected = false;
                return;
            }
        }

        private void OnReciveDataCallback(IAsyncResult result)
        {
            try
            {
                int len = stream.EndRead(result);

                byte[] recivedBytes = new byte[len];
                Buffer.BlockCopy(asyncBuffer, 0, recivedBytes, 0, len);

                if (len == 0)
                {
                    Logger.Error("disconnected");
                    return;
                }

                if (DataRecived != null)
                    DataRecived(new ByteBuffer(recivedBytes));

                stream.BeginRead(asyncBuffer, 0, 8192, OnReciveDataCallback, null);
            }
            catch (Exception ex)
            {
                Logger.Critical($"Can't recive data: {ex}");
                throw;
            }
        }

        public void SendData(ByteBuffer data)
        {
            stream.BeginWrite(data.ToArray, 0, data.ToArray.Length, null, null);
        }

        public void CloseConnection() =>
            client.Close();
    }
}