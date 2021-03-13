using HtmlAgilityPack;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockLib
{

    public class DividendInfo
    {
        private readonly static string XpathDividendHeader = "/html/body/table[2]/tbody/tr/td[3]/div[2]/div/div/table/tbody/tr";
        private readonly static string Url = "https://goodinfo.tw/StockInfo/StockDividendPolicy.asp?STOCK_ID=";
        public DividendInfo()
        {

        }
        public DividendInfo(List<HtmlNode> nodes)
        {
            DividendProvideYear = nodes[0].ParseNodeToInt();
            CashDividendSurplus = nodes[1].ParseNodeToFloat();
            CashDividendProvident = nodes[2].ParseNodeToFloat();
            CashDividendTotal = nodes[3].ParseNodeToFloat();
            StockDividendSurplus = nodes[4].ParseNodeToFloat();
            StockDividendProvident = nodes[5].ParseNodeToFloat();
            StockDividendTotal = nodes[6].ParseNodeToFloat();
            DividendTotal = nodes[7].ParseNodeToFloat();
            DividendTotalCash = nodes[8].ParseNodeToFloat();
            DividendTotalStock = nodes[9].ParseNodeToFloat();
            FillDividendDays = nodes[10].ParseNodeToFloat();
            FillStockDays = nodes[11].ParseNodeToFloat();
            StockPriceYear = nodes[12].ParseNodeToInt();
            Highest = nodes[13].ParseNodeToFloat();
            Lowest = nodes[14].ParseNodeToFloat();
            Average = nodes[15].ParseNodeToFloat();
            AnnualAverageDividendYield_Cash = nodes[16].ParseNodeToFloat();
            AnnualAverageDividendYield_Stock = nodes[17].ParseNodeToFloat();
            AnnualAverageDividendYield_Total = nodes[18].ParseNodeToFloat();
            DividendYear = nodes[19].ParseNodeToInt();
            EPS = nodes[20].ParseNodeToFloat();
            PayoutRatio_Cash = nodes[21].ParseNodeToFloat();
            PayoutRatio_Stock = nodes[22].ParseNodeToFloat();
            PayoutRatio_Total = nodes[23].ParseNodeToFloat();
        }
        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股利發放年度")]
        public int? DividendProvideYear { get; set; }

        #region 股利政策
        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("現金股利-盈餘")]
        public double? CashDividendSurplus { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("現金股利-公積")]
        public double? CashDividendProvident { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("現金股利-合計")]
        public double? CashDividendTotal { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股票股利-盈餘")]
        public double? StockDividendSurplus { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股票股利-公積")]
        public double? StockDividendProvident { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股票股利-合計")]
        public double? StockDividendTotal { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股利合計")]
        public double? DividendTotal { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股利總計現金(億)")]
        public double? DividendTotalCash { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股利總計股票(千張)")]
        public double? DividendTotalStock { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("填息花費日數")]
        public double? FillDividendDays { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("填權花費日數")]
        public double? FillStockDays { get; set; }
        #endregion
        #region 殖利率統計
        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股價年度")]
        public int? StockPriceYear { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股價年度最高")]
        public double? Highest { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股價年度最低")]
        public double? Lowest { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股價年度平均")]
        public double? Average { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("年均殖利率(%)-現金")]
        public double? AnnualAverageDividendYield_Cash { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("年均殖利率(%)-股票")]
        public double? AnnualAverageDividendYield_Stock { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("年均殖利率(%)-合計")]
        public double? AnnualAverageDividendYield_Total { get; set; }
        #endregion
        #region 盈餘分配統計
        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("股利所屬年度")]
        public int? DividendYear { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("EPS(元)")]
        public double? EPS { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("盈餘分配率(%)-配息")]
        public double? PayoutRatio_Cash { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("盈餘分配率(%)-配股")]
        public double? PayoutRatio_Stock { get; set; }

        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:0.##}")]
        [DisplayName("盈餘分配率(%)-合計")]
        public double? PayoutRatio_Total { get; set; }
        #endregion




        public static async Task<List<DividendInfo>> GetCompanyDividend(string stockID)
        {
            var web = new HtmlWeb
            {
                UserAgent = Define.Agent,
                OverrideEncoding = Encoding.UTF8
            };
            var doc = await web.LoadFromWebAsync(Url + stockID);
            var nodes = doc.DocumentNode.SelectNodes(Regex.Replace(XpathDividendHeader, "/tbody([[]\\d[]])?", ""));
            //最後一筆為累計值 
            return nodes.Take(nodes.Count - 1).Select(x => x.ChildNodes.Where(x => x.Name == "td").ToList())
               .Where(nodes => nodes[0].InnerText != "∟").Select(nodes => new DividendInfo(nodes)).ToList();
        }
    }

}
