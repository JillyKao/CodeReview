using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketLib
{
    public static class SocketExtension
    {
        public static byte[] GetDataWithHead(this byte[] datas)
        {
            var length = (uint)datas.Length;
            IEnumerable<byte[]> getDatas()
            {
                yield return BitConverter.GetBytes(length);//計算此次封包的長度 最長為uint.Maxvalue
                yield return datas;
            }
            return getDatas().SelectMany(x => x).ToArray();
        }
    }
}
