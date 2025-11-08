using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            var assets = new List<AssetCategory>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var lastUpdate = GetLastUpdate(doc);
                if (lastUpdate == null)
                {

                    return new List<AssetCategory>();
                }

                var table = GetTableByCategory(doc, category);
                if (table == null)
                {
                    return new List<AssetCategory>();
                }

                return ParseRows(table, category, lastUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TableParser] Failed to parse table: {ex.Message}");
                return assets;
            }
        }

        private static string? GetLastUpdate(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//td[contains(translate(text(),'last update','LAST UPDATE'),'LAST UPDATE')]");
            return node?.InnerText.Trim();
        }

        private static HtmlNode? GetTableByCategory(HtmlDocument doc, string category)
        {
            string categoryUpper = category.ToUpper();
            var tableNameNode = doc.DocumentNode.SelectSingleNode($"//table//tr//th//b[contains(translate(text(),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'{categoryUpper}')]");
            return tableNameNode?.Ancestors("table").FirstOrDefault();
        }

        private static (string AssetName, string AssetId)? ParseAssetText(string assetText)
        {
            var match = Regex.Match(assetText, @"^(.*?)\s*\((.*?)\)\*?$");
            if (!match.Success) return null;
            return (match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim());
        }
        private static List<AssetCategory> ParseRows(HtmlNode table, string category, string lastUpdate)
        {
            var assets = new List<AssetCategory>();
            var rows = table.SelectNodes(".//tr");

            if (rows == null || rows.Count < 3)
            {
                return assets;
            }

            foreach (var row in rows.Skip(2))
            {
                try
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells == null || cells.Count < 3) { continue; }

                    string assetText = cells[0].InnerText.Trim();
                    string tng = cells[1].InnerText.Trim();

                    var parsed = ParseAssetText(assetText);
                    if (parsed == null) { continue; }

                    assets.Add(new AssetCategory
                    {
                        LastUpdate = lastUpdate,
                        CategoryName = category.ToUpper(),
                        AssetName = parsed.Value.AssetName,
                        AssetId = parsed.Value.AssetId,
                        TotalNetGeneration = tng
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[TableParser] Failed to parse row: {ex.Message}");
                }
            }

            return assets;
        }
    }
}
