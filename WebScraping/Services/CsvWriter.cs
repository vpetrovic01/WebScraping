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
                if (assets == null || assets.Count == 0)
                {
                    Console.WriteLine("[CsvWriter] No assets to write");
                    return;
                }

                var sb = new StringBuilder();

                sb.AppendLine("LastUpdate,AssetName,AssetId,TotalNetGeneration");

                foreach (var asset in assets)
                {
                    sb.AppendLine($"\"{asset.LastUpdate}\",\"{asset.CategoryName} - {asset.AssetName}\",\"{asset.AssetId}\",\"{asset.TotalNetGeneration}\"");
                }
                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"CSV created: {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[CsvWriter] File write failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CsvWriter] Unexpected error: {ex.Message}");
            }

        }
    }
}
