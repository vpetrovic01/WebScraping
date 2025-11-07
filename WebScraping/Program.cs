using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
class Program
{
    static async Task Main()
    {
        var url = "http://ets.aeso.ca/ets_web/ip/Market/Reports/CSDReportServlet";
        using var client = new HttpClient();
        string html = await client.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var lastUpdateNode = doc.DocumentNode.SelectSingleNode(
        "//td[contains(text(),'Last Update') or contains(text(),'LAST UPDATE')]"
    );
        if (lastUpdateNode == null)
        {
            Console.WriteLine("Can't find last update field");
            return;
        }

        var wind = doc.DocumentNode.SelectSingleNode(
       "//table//tr//th//b[contains(translate(text(), 'wind', 'WIND'), 'WIND')]"
        );
        if (wind == null)
        {
            Console.WriteLine("Can't find table named Wind");
            return;
        }
        HtmlNode windTable = wind.Ancestors("table").FirstOrDefault();
        if (windTable == null)
        {
            Console.WriteLine("Can't find parent table");
            return;
        }
        var rows = windTable.SelectNodes(".//tr");

        if (rows == null || rows.Count < 2)
        {
            Console.WriteLine("Not enough data in the table");
            return;
        }
        string csvFile = "wind_data.csv";
        using (var writer = new StreamWriter(csvFile))
        {
            writer.WriteLine("LastUpdate,Asset,AssetId,TotalNetGeneration");
            foreach (var row in rows.Skip(2))
            {
                var cells = row.SelectNodes(".//td");
                if (cells == null || cells.Count < 3)
                {
                    continue;
                }
                string assetText = cells[0].InnerText.Trim();

                var match = Regex.Match(assetText, @"^(.*?)\s*\((.*?)\)\*?$");

                if (!match.Success)
                {
                    Console.WriteLine("Can't parse");
                    return;
                }
                string lastUpdate = lastUpdateNode.InnerText.Trim();
                string windStr = wind.InnerText.Trim();
                string assetName = match.Groups[1].Value;
                string assetId = match.Groups[2].Value;
                string tng = cells[2].InnerText;

                string line = $"\"{lastUpdate}\",\"{windStr}\",\"{assetName}\",\"{assetId}\",\"{tng}\"";
                writer.WriteLine(line);
            }
        }
        Console.WriteLine($"CSV file created: {csvFile}");
    }
}