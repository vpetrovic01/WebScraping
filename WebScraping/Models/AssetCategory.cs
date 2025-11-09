using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Models
{
    public class AssetCategory
    {
        public string LastUpdate { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string AssetId { get; set; } = string.Empty;
        public string TotalNetGeneration { get; set; } = string.Empty;
    }
}
