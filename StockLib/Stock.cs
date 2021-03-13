using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockLib
{
    public class Stock
    {
        private static readonly string GroupUrl = @"https://goodinfo.tw/StockInfo/StockList.asp";
        private static readonly string GroupXPath = @"/html/body/table[5]/tbody/tr/td[3]/div[1]/table[2]/tbody";

        private static readonly string StockUrl = @"https://goodinfo.tw/StockInfo/StockList.asp?MARKET_CAT=%E5%85%A8%E9%83%A8&STOCK_CODE=";
        private static readonly string StockXPath = "/html/body/table[5]/tbody/tr/td[3]/div[2]/div/div/table[1]";
        /// <summary>
        /// 股票編號
        /// </summary>
        public string StockID { get; set; }
        /// <summary>
        /// 股票名稱
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 上市櫃
        /// </summary>
        public string ListingCounter { get; set; }
        /// <summary>
        /// 這個要有點久
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Stock>> GetAllStock()
        {
            async Task<List<string>> getGroupID()
            {
                var web = new HtmlWeb
                {
                    UserAgent = Define.Agent,
                    OverrideEncoding = Encoding.UTF8
                };
                var doc = await web.LoadFromWebAsync(GroupUrl);
                return doc.DocumentNode.SelectNodes(Regex.Replace(GroupXPath, "/tbody([[]\\d[]])?", ""))
                    .SelectMany(x => x.ChildNodes.Where(c =>
                    {
                        return c.Name == "tr" && c.InnerHtml.Contains("STOCK_CODE");
                    }).SelectMany(n =>
                    {
                        return n.ChildNodes.Select(c => c.InnerText).ToList();
                    }).Where(x => x != "&nbsp;").ToList()).ToList();
            }
            async Task<List<Stock>> getStock(string code)
            {
                try
                {
                    var web = new HtmlWeb
                    {
                        UserAgent = Define.Agent,
                        OverrideEncoding = Encoding.UTF8
                    };
                    var doc = await web.LoadFromWebAsync(StockUrl + code);
                    return doc.DocumentNode.SelectNodes(Regex.Replace(StockXPath, "/tbody([[]\\d[]])?", ""))
                        .SelectMany(x => x.ChildNodes.Where(c => c.Name == "tr")
                        .Select(n => new Stock()
                        {
                            StockID = n.ChildNodes[0].InnerText,
                            StockName = n.ChildNodes[1].InnerText,
                            ListingCounter = n.ChildNodes[2].InnerText,
                        }).ToList()).Distinct().ToList();
                }
                catch(Exception e)
                {
                    throw e;
                }

            }
            var groupIDs = await getGroupID();
            await Task.Delay(TimeSpan.FromSeconds(60));
            var result = new List<Stock>();
            foreach (var code in groupIDs)
            {
                result.AddRange(await getStock(code));
                await Task.Delay(TimeSpan.FromSeconds(60));
            }
            return result;
        }
    }
}
