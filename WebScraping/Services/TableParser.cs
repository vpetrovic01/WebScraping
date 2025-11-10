using HtmlAgilityPack;
using System.Text.RegularExpressions;
using WebScraping.Models;

namespace WebScraping.Services
{
    public class TableParser
    {
        public static List<AssetCategory> Parse(string html, string category)
        {
            var assets = new List<AssetCategory>();
            try
            {
                Logger.Info($"[TableParser] Parsing table for category '{category}'");
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var lastUpdate = GetLastUpdate(doc);
                if (lastUpdate == null)
                {
                    Logger.Error("[TableParser] Cannot find lastUpdate field");
                    return new List<AssetCategory>();
                }

                var table = GetTableByCategory(doc, category);
                if (table == null)
                {
                    Logger.Error($"[TableParser] Cannot find table for category '{category}'");
                    return new List<AssetCategory>();
                }

                assets = ParseRows(table, category, lastUpdate);
                Logger.Info($"[TableParser] Parsed {assets.Count} assets from category '{category}'");
                return assets;
            }
            catch (Exception ex)
            {
                Logger.Error($"[TableParser] Failed to parse table: {ex.Message}");
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
                Logger.Error($"[TableParser] Table for category '{category}' has insufficient rows or is missing.");
                throw new Exception();
            }

            foreach (var row in rows.Skip(2))
            {
                try
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells == null || cells.Count < 3)
                    {
                        Logger.Error($"[TableParser] Table for category '{category}' has insufficient columns or is missing.");
                        continue;
                    }

                    string assetText = cells[0].InnerText.Trim();
                    string tng = cells[2].InnerText.Trim();

                    var parsed = ParseAssetText(assetText);
                    if (parsed == null)
                    {
                        Logger.Error("[TableParser] Unable to parse assetName and assetId.");
                        continue;
                    }

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
                    Logger.Error($"[TableParser] Failed to parse row: '{row.InnerText.Trim()}'. {ex.Message}");
                }
            }

            return assets;
        }
    }
}
