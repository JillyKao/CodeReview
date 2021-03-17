using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketLib
{
    public class TcpSocketServer
    {
        private readonly IPAddress _IPAddress;
        private readonly int _Port;
        private Socket _Listener;
        public TcpSocketServer(IPAddress IPAddress, int Port)
        {
            _IPAddress = IPAddress;
            _Port = Port;
        }
        private List<SocketObj> Clients { get; set; } = new List<SocketObj>();
        public event Action<SocketObj> OnClientAccept;
        public void SendToAll(byte[] datas)
        {
            var dataswithHead = datas.GetDataWithHead();
            Parallel.ForEach(Clients.ToArray(), async client =>
              {
                  await client.SendAsyncNotSetHead(dataswithHead);
              });
        }

        public async Task StartListening()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(_IPAddress, _Port);
                _Listener = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);

                _Listener.Bind(localEndPoint);
                _Listener.Listen();

                while (true)
                {
                    var socket = await _Listener.AcceptAsync();

                    var clientObj = new SocketObj(socket);
                    OnClientAccept?.Invoke(clientObj);

                    clientObj.OnDisconnect += (co) =>
                    {
                        Clients.Remove(co);
                        Console.WriteLine("Client Disconnect!");
                    };
                    Clients.Add(clientObj);
                    Console.WriteLine("New Client Accept!");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Server Closeing!");
            }
        }


    }
}
