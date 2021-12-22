using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedClientServer
{
    public abstract class Protocol<TMessageType>
    {
        const int HEADER_SIZE = 4;
        public async Task<TMessageType> ReceiveAsync(NetworkStream networkStream)
        {
            var bodyLenth = await ReadHeader(networkStream).ConfigureAwait(false);

            AssertValidMessageLength(bodyLenth);

            return await ReadBody(networkStream, bodyLenth).ConfigureAwait(false);
        }
        public async Task SendAsync<T>(NetworkStream networkStream, T message)
        {
            var (header, body) = Encode(message);
            await networkStream.WriteAsync(header, 0, header.Length);
            await networkStream.WriteAsync(body, 0, body.Length);
        }
        async Task<TMessageType> ReadBody(NetworkStream networkStream, int bodyLenth)
        {
            var bodyBytes = await ReadAsync(networkStream, bodyLenth).ConfigureAwait(false);

            return Decode(bodyBytes);
        }

        async Task<int> ReadHeader(NetworkStream networkStream)
        {
            var headerBytes = await ReadAsync(networkStream, HEADER_SIZE);

            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerBytes));
        }
        
        async Task<byte[]> ReadAsync(NetworkStream networkstream, int bytesToRead)
        {
            var buffer = new byte[bytesToRead];
            var bytesRead = 0;

            while (bytesRead < bytesToRead)
            {
                var tempByte = await networkstream.ReadAsync(buffer, bytesRead, bytesToRead - bytesRead).ConfigureAwait(false);

                if (tempByte == 0)
                    throw new Exception("Socket closed");

                bytesRead += tempByte;
            }

            return buffer;
        }

        public (byte[] header, byte[] body) Encode<T>(T message)
        {
            var bodyBytes = EncodeBody<T>(message);
            var headerBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bodyBytes.Length));
            return (headerBytes, bodyBytes);
        }

        protected abstract TMessageType Decode(byte[] message);
        protected abstract byte[] EncodeBody<T>(T message);
        protected virtual void AssertValidMessageLength(int messageLenth)
        {
            if (messageLenth < 1)
                throw new ArgumentOutOfRangeException("Invalid message length");
        }
    }
}
