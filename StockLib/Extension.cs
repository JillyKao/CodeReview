using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockLib
{
    internal static class HtmlNodeExtension
    {
        public static int? ParseNodeToInt(this HtmlNode node) =>
             int.TryParse(node.InnerText, out var result) ? result : null;
        public static float? ParseNodeToFloat(this HtmlNode node) =>
             float.TryParse(node.InnerText, out var result) ? result : null;
    }
}
