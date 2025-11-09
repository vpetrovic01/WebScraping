using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Services;
class Program
{
    static async Task Main()
    {
        var url = "http://ets.aeso.ca/ets_web/ip/Market/Reports/CSDReportServlet";

        try
        {
            string html = await HtmlFetcher.FetchHtml(url);

            var tableName = "Wind";

            var assets = TableParser.Parse(html, tableName);

            if (assets == null || assets.Count == 0)
            {
                Console.WriteLine("No assets found");
                return;
            }

            string timestamp = DateTimeParser.ParseLastUpdateDate(assets);

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{tableName}_data_{timestamp}.csv");
            
            CsvWriter.TryWrite(filePath, assets);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }
}