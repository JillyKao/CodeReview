using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketLib
{
    public class TcpSocketClient : SocketObj
    {

        private readonly IPAddress _IPAddress;
        private readonly int _Port;

        public int ConnectTimeOut = 10;
        public TcpSocketClient(IPAddress IPAddress, int Port) : base(new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream, ProtocolType.Tcp))
        {
            _IPAddress = IPAddress;
            _Port = Port;
        }

        public TcpSocketClient(string IPString, int Port) : base(new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream, ProtocolType.Tcp))
        {
            _IPAddress = IPAddress.Parse(IPString);
            _Port = Port;
        }

        public void ResetSocket()
        {
            this.Socket = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task<bool> StartClient()
        {
            try
            {
                IPEndPoint RemoteEP = new IPEndPoint(_IPAddress, _Port);
                Socket.SendTimeout = 3000;
                Socket.ReceiveTimeout = 3000;
                await Socket.ConnectAsync(RemoteEP);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
