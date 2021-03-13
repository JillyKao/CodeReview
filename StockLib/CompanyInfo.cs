using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockLib
{
    public class CompanyInfo
    {
        private static readonly string Url =
            "https://goodinfo.tw/StockInfo/StockDetail.asp?STOCK_ID=";
        private static readonly string XpathCompanyName = "/html/body/table[2]/tbody/tr/td[3]/table/tbody/tr[2]/td[3]/table[2]/tbody/tr[1]/td[2]";
        private static readonly string XpathCompanyIndustry = "/html/body/table[2]/tbody/tr/td[3]/table/tbody/tr[2]/td[3]/table[2]/tbody/tr[2]/td[2]";
        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 產業別
        /// </summary>
        public string CompanyIndustry { get; set; }

        public static async Task<CompanyInfo> GetInfo(string stockID)
        {
            var info = new CompanyInfo();
            var web = new HtmlWeb
            {
                UserAgent = Define.Agent,
                OverrideEncoding = Encoding.UTF8
            };

            var doc = await web.LoadFromWebAsync(Url + stockID);
            {
                var nodes = doc.DocumentNode.SelectNodes(Regex.Replace(XpathCompanyName, "/tbody([[]\\d[]])?", ""));
                info.CompanyName = nodes?[0].ChildNodes?[0].InnerText ?? "---";
            }
            {
                var nodes = doc.DocumentNode.SelectNodes(Regex.Replace(XpathCompanyIndustry, "/tbody([[]\\d[]])?", ""));
                info.CompanyIndustry = nodes?[0].ChildNodes?[0].InnerText ?? "---";
            }
            return info;
        }

    }
}
