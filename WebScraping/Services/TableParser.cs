using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Models;

namespace WebScraping.Services
{
    internal class TableParser
    {
        public static List<AssetCategory> Parse(string html, string category)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var assets = new List<AssetCategory>();

            string categoryUpper = category.ToUpper();

            var lastUpdateNode = doc.DocumentNode.SelectSingleNode("//td[contains(translate(text(),'last update', 'LAST UPDATE'),'LAST UPDATE')]");
            var tableName = doc.DocumentNode.SelectSingleNode($"//table//tr//th//b[contains(translate(text(),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'{categoryUpper}')]");

            if (lastUpdateNode == null || tableName == null)
            {
                return assets;
            }

            string lastUpdate = lastUpdateNode.InnerText.Trim();

            var table = tableName.Ancestors("table").FirstOrDefault();
            var rows = table?.SelectNodes(".//tr");

            if (rows == null || rows.Count < 3)
            {
                return assets;
            }

            foreach (var row in rows.Skip(2))
            {
                var cells = row.SelectNodes(".//td");
                if (cells == null || cells.Count < 3)
                    continue;

                string assetText = cells[0].InnerText.Trim();
                string tng = cells[2].InnerText.Trim();

                var match = Regex.Match(assetText, @"^(.*?)\s*\((.*?)\)\*?$");
                if (!match.Success)
                    continue;

                assets.Add(new AssetCategory
                {
                    LastUpdate = lastUpdate,
                    CategoryName = categoryUpper,
                    AssetName = match.Groups[1].Value.Trim(),
                    AssetId = match.Groups[2].Value.Trim(),
                    TotalNetGeneration = tng
                });
            }

            return assets;
        }

    }
}
