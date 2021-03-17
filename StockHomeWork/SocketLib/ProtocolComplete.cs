using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketLib
{
    /// <summary>
    /// 避免封包斷掉 取完完整的封包再呼叫事件 固定格式為 byte[] 前四個位置為 uint 代表需要取得的封包長度
    /// </summary>
    public class ProtocolComplete
    {
        public event Action<byte[]> CompleteProtocolEvent;
        private readonly List<byte> TempData = new List<byte>();
        private int ReceiveLength = -1;
        public void ReceiveData(IEnumerable<byte> Data)
        {
            TempData.AddRange(Data);
            if (ReceiveLength < 0 && TempData.Count >= 4)
            {
                ReceiveLength = BitConverter.ToInt32(TempData.GetRange(0, 4).ToArray(), 0);
                TempData.RemoveRange(0, 4);
            }
            if (ReceiveLength >= 4)
            {
                if (ReceiveLength <= TempData.Count)
                {
                    byte[] aFullData = TempData.GetRange(0, ReceiveLength).ToArray();
                    TempData.RemoveRange(0, ReceiveLength);
                    if (CompleteProtocolEvent != null)
                        CompleteProtocolEvent(aFullData);
                    ReceiveLength = -1;
                }
            }
            else
            {
                throw new Exception("ReceiveLength Error! " + ReceiveLength);
            }
        }
    }
}
