using System;
using System.Text;
using System.Collections.Generic;

namespace Connection
{
    public class ByteBuffer : IDisposable
    {
        private List<byte> Buffer;
        public byte[] readBuffer { get; private set; }
        public int readPosition { get; private set; }
        private bool bufferUpdated = false;

        public ByteBuffer()
        {
            Buffer = new List<byte>();
            readPosition = 0;
        }

        public byte[] ToArray { get => Buffer.ToArray(); }
        public int Count { get => Buffer.Count; }
        public int Length { get => Count - readPosition; }

        public void Clear()
        {
            Buffer.Clear();
            readPosition = 0;
        }

        #region Write

        public void WriteBytes(byte[] input)
        {
            Buffer.AddRange(input);
            bufferUpdated = true;
        }

        public void WriteShort(short input)
        {
            Buffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void WriteInt(int input)
        {
            Buffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void WriteFloat(float input)
        {
            Buffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void WriteLong(long input)
        {
            Buffer.AddRange(BitConverter.GetBytes(input));
            bufferUpdated = true;
        }

        public void WriteString(string input)
        {
            Buffer.AddRange(BitConverter.GetBytes(input.Length));
            Buffer.AddRange(Encoding.ASCII.GetBytes(input));
            bufferUpdated = true;
        }

        #endregion
    
        #region Read

        public int ReadInt(bool peek = true)
        {
            if (Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = ToArray;
                    bufferUpdated = false;
                }
                int ret = BitConverter.ToInt32(readBuffer, readPosition);
                if (peek & Count > readPosition)
                    readPosition += 4;
                return ret;
            }
            else
                throw new Exception("Byte buffer is past limit");
        }

        public byte[] ReadBytes(int length, bool peek = true)
        {
            if (bufferUpdated)
            {
                readBuffer = ToArray;
                bufferUpdated = false;
            }
            byte[] ret = Buffer.GetRange(readPosition, length).ToArray();
            if (peek)
                readPosition += 4;
            return ret;
        }

        public string ReadString(bool peek = true)
        {
            int Len = ReadInt();
            if (bufferUpdated)
            {
                readBuffer = ToArray;
                bufferUpdated = false;
            }
            string ret = Encoding.ASCII.GetString(readBuffer, readPosition, Len);
            if (peek & Count > readPosition && ret.Length > 0)
                    readPosition += Len;
            return ret;
        }
        
        public short ReadShort(bool peek = true)
        {
            if (Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = ToArray;
                    bufferUpdated = false;
                }
                short ret = BitConverter.ToInt16(readBuffer, readPosition);
                if (peek & Count > readPosition)
                    readPosition += 2;
                return ret;
            }
            else
                throw new Exception("Byte buffer is past limit");
        }

        public float ReadFloat(bool peek = true)
        {
            if (Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = ToArray;
                    bufferUpdated = false;
                }
                float ret = BitConverter.ToSingle(readBuffer, readPosition);
                if (peek & Count > readPosition)
                    readPosition += 4;
                return ret;
            }
            else
                throw new Exception("Byte buffer is past limit");
        }

        public long ReadLong(bool peek = true)
        {
            if (Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = ToArray;
                    bufferUpdated = false;
                }
                long ret = BitConverter.ToInt64(readBuffer, readPosition);
                if (peek & Count > readPosition)
                    readPosition += 8;
                return ret;
            }
            else
                throw new Exception("Byte buffer is past limit");
        }

        #endregion

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    Buffer.Clear();
                readPosition = 0;
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}