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
        string html = await HtmlFetcher.fetchHtml(url);

        var assets = TableParser.Parse(html, "wind");

        if (assets == null)
        {
            Console.WriteLine("No assets found");
            return;
        }

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.csv");
        CsvWriter.Write(filePath, assets);
        Console.WriteLine($"CSV created: {filePath}");

    }
}