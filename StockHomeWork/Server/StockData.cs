using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class StockData
    {
        public static List<StockData> StockDatas { get; private set; }

        public static void IniStock()
        {
            StockDatas = Enumerable.Range(0, 100).Select(x => new StockData() { ID = x }).ToList();
        }
        public double ClosingPrice { get; set; }

        public string Name { get; set; }

        public int ID { get; set; }
        public double? Price { get; set; }
        private const string Chars = "abdefghjknpqrstuwyABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private readonly Random Random = new Random();
        public StockData()
        {
            this.ClosingPrice = Math.Round(Random.Next(10, 3000) + Random.NextDouble(), 2);//隨機收盤價10-2999
            this.Name = new string(Enumerable.Range(0, 4)
                  .Select(s => Chars[Random.Next(Chars.Length)]).ToArray());//隨機名稱
            UpdatePrice();
        }


        public void UpdatePrice()
        {
            var r = Random.Next(-10, 10);
            Price = Math.Round(ClosingPrice + ClosingPrice * r / 100d, 2);//收盤價的10%上下
        }
    }
}
