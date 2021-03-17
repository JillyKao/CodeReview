using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketLib
{
    public class SocketObj
    {
        protected Socket Socket { get; set; }
        /// <summary>
        /// 送出資料前加上資料長度的Head
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public async Task SendAsync(byte[] datas)
        {
            var data = new ArraySegment<byte>(datas.GetDataWithHead());
            await Socket.SendAsync(data, SocketFlags.None);
        }
        /// <summary>
        /// 不預先計算資料長度 直接送出
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public async Task SendAsyncNotSetHead(byte[] datas)
        {
            try
            {
                var data = new ArraySegment<byte>(datas);
                await Socket.SendAsync(data, SocketFlags.None);
            }
            catch(Exception e)
            {
                OnDisconnect?.Invoke(this);
            }
        }
        private ProtocolComplete ProtocolComplete { get; set; }
        public event Action<byte[]> OnDataReceive;
        public event Action<SocketObj> OnDisconnect;
        public SocketObj(Socket socket)
        {
            this.Socket = socket;
            ProtocolComplete = new ProtocolComplete();
            ProtocolComplete.CompleteProtocolEvent += (datas) => OnDataReceive?.Invoke(datas);
        }

        public async Task StartReceiveAsync()
        {
            var buffer = new byte[2048];
            var data = new ArraySegment<byte>(buffer);

            while (true)
            {
                try
                {
                    var count = await Socket.ReceiveAsync(data, SocketFlags.None);
                    ProtocolComplete.ReceiveData(buffer.Take(count));
                }
                catch (Exception e)
                {
                    OnDisconnect?.Invoke(this);
                    //TODO 斷線
                    return;
                }
            }
        }
    }
}
