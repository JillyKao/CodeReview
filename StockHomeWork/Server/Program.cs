using Newtonsoft.Json;
using Protocol;
using SocketLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StockData.IniStock();
            float updateRate = 0.5f;
            string IP = "127.0.0.1";
            int Port = 2001;
            _ = Task.Run(async () =>
                {
                    var server = new TcpSocketServer(IPAddress.Parse(IP), Port);
                    server.OnClientAccept += async (clientObj) =>
                     {
                         clientObj.OnDataReceive += async (datas) =>
                         {
                             var code = (ProtocolDefine)datas[0];
                             switch (code)
                             {
                                 case ProtocolDefine.RequestStockData://接收到封包請求回傳股票資料(ID Name 收盤價)
                                     {
                                         var json = JsonConvert.SerializeObject(StockData.StockDatas.Select(x =>
                                         {
                                             return new { x.ID, x.Name, x.ClosingPrice };
                                         }).ToArray());

                                         IEnumerable<byte[]> attackCode()
                                         {
                                             yield return BitConverter.GetBytes((int)ProtocolDefine.ResponseStockData);
                                             yield return Encoding.UTF8.GetBytes(json);
                                         }
                                         await clientObj.SendAsync(attackCode().SelectMany(x => x).ToArray());
                                     }
                                     break;
                                 default:
                                     {
                                         Console.WriteLine("Un Define Code");
                                     }
                                     break;
                             }
                         };

                         await clientObj.StartReceiveAsync();
                     };
                    //更新資料 並回傳所有client 只傳送(ID 最後成交價)
                    _ = Task.Run(async () =>
                    {
                        while (true)
                        {
                            try
                            {
                                Parallel.ForEach(StockData.StockDatas, x => x.UpdatePrice());
                                var json = JsonConvert.SerializeObject(StockData.StockDatas.Select(x =>
                                {
                                    return new { x.ID, x.Price };
                                }).ToArray());

                                IEnumerable<byte[]> attackCode()
                                {
                                    yield return BitConverter.GetBytes((int)ProtocolDefine.UpdateStockData);
                                    yield return Encoding.UTF8.GetBytes(json);
                                }
                                server.SendToAll(attackCode().SelectMany(x => x).ToArray());
                                await Task.Delay(TimeSpan.FromSeconds(updateRate));
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    });
                    await server.StartListening();//開始接受接入請求
                });


            while (true)
            {
                Console.WriteLine($"輸入Exit離開,輸入秒數可控制更新間隔 當前間隔為 {updateRate} 秒");
                var line = Console.ReadLine();
                if (line == "Exit")
                {
                    return;
                }
                if (float.TryParse(line, out var rate))
                {
                    updateRate = rate;
                }
                else
                {
                    Console.WriteLine("無效指令");
                }
            }

        }
    }
}
