using System.Text;
using WebScraping.Models;

namespace WebScraping.Services
{
    public class CsvWriter
    {
        public static bool TryWrite(string filePath, List<AssetCategory> assets)
        {
            try
            {
                Logger.Info($"[CsvWriter] Writing data into csv file.");
                if (assets == null || assets.Count == 0)
                {
                    Logger.Error("[CsvWriter] No assets to write");
                    return false;
                }

                var sb = new StringBuilder();

                sb.AppendLine("LastUpdate,AssetName,AssetId,TotalNetGeneration");

                foreach (var asset in assets)
                {
                    sb.AppendLine($"\"{asset.LastUpdate}\",\"{asset.CategoryName} - {asset.AssetName}\",\"{asset.AssetId}\",\"{asset.TotalNetGeneration}\"");
                }
                File.WriteAllText(filePath, sb.ToString());
                Logger.Info($"[CsvWriter] CSV created: {filePath}");
                return true;
            }
            catch (IOException ex)
            {
                Logger.Error($"[CsvWriter] File write failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"[CsvWriter] Unexpected error: {ex.Message}");
                return false;
            }

        }
    }
}
