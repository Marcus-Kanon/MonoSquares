using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedClientServer
{
    public class ClientChannel<TProtocol, TMessageType> : Channel<TProtocol, TMessageType>, IDisposable
        where TProtocol : Protocol<TMessageType>, new()
    {
        public async Task ConnectAsync(IPEndPoint endPoint)
        {
            var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(endPoint).ConfigureAwait(false);
            Attach(socket);

        }


    }
}
