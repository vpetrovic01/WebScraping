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
            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }
    }
}
