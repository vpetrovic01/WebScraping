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

            using var writer = new StreamWriter(filePath);
            writer.WriteLine("LastUpdate, AssetName, AssetId, TotalNetGeneration");

            foreach (var asset in assets)
            {
                writer.WriteLine($"\"{asset.LastUpdate}\",\"{asset.CategoryName} - {asset.AssetName}\",\"{asset.AssetId}\",\"{asset.TotalNetGeneration}\"");
            }

        }
    }
}
