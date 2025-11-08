using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Services
{
    internal class HtmlFetcher
    {
        public static async Task<string> FetchHtml(string url)
        {
            try
            {
                using var client = new HttpClient();
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[HtmlFetcher] Web request failed: {ex.Message}");
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HtmlFetcher] Unexpected error: {ex.Message}");
                return "";
            }
        }
    }
}
