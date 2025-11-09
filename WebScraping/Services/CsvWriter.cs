using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Models;

namespace WebScraping.Services
{
    internal class CsvWriter
    {
        public static void Write(string filePath, List<AssetCategory> assets)
        {
            try
            {
                Logger.Info($"[CsvWriter] Writing data into csv file.");
                if (assets == null || assets.Count == 0)
                {
                    Logger.Error("[CsvWriter] No assets to write");
                    return;
                }

                var sb = new StringBuilder();

                sb.AppendLine("LastUpdate,AssetName,AssetId,TotalNetGeneration");

                foreach (var asset in assets)
                {
                    sb.AppendLine($"\"{asset.LastUpdate}\",\"{asset.CategoryName} - {asset.AssetName}\",\"{asset.AssetId}\",\"{asset.TotalNetGeneration}\"");
                }
                File.WriteAllText(filePath, sb.ToString());
                Logger.Info($"[CsvWriter] CSV created: {filePath}");
            }
            catch (IOException ex)
            {
                Logger.Error($"[CsvWriter] File write failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Error($"[CsvWriter] Unexpected error: {ex.Message}");
            }

        }
    }
}
