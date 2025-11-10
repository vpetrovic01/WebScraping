using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Models;

namespace WebScraping.Services
{
    public class DateTimeParser
    {
        public static string ParseLastUpdateDate(string lastUpdate)
        {
            Logger.Info("[DateTimeParser] Parsing date and time");

            if (lastUpdate == null || lastUpdate == "")
            {
                throw new ArgumentException("[DateTimeParser] Asset list is empty");
            }

            string timestamp;

            string[] formats = { "'Last Update :' MMM dd, yyyy HH:mm", "Last Update: MMMM dd, yyyy HH:mm" };

            if (DateTime.TryParseExact(lastUpdate, formats, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate))
            {
                timestamp = parsedDate.ToString("yyyyMMdd_HHmm");

            }
            else
            {
                Logger.Warning($"[DateTimeParser] Failed to parse datetime from '{lastUpdate}', using current time.");
                timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
            }
            Logger.Info("[DateTimeParser] Successfully parsed date and time.");
            return timestamp;
        }
    }
}
