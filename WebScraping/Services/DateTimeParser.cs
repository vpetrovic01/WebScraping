using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Models;

namespace WebScraping.Services
{
    internal class DateTimeParser
    {
        public static string ParseLastUpdateDate(List<AssetCategory> assets)
        {
            var lastUpdate = assets[2].LastUpdate;

            string timestamp;

            string[] formats = { "'Last Update :' MMM dd, yyyy HH:mm", "MMM dd, yyyy HH:mm" };

            if (DateTime.TryParseExact(lastUpdate, formats, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate))
            {
                timestamp = parsedDate.ToString("yyyyMMdd_HHmm");

            }
            else
            {
                timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }

            return timestamp;
        }
    }
}
