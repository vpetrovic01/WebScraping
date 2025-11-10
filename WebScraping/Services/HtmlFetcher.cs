namespace WebScraping.Services
{
    public class HtmlFetcher
    {
        public static async Task<string> FetchHtml(string url)
        {
            try
            {
                Logger.Info($"[HtmlFetcher] Fetching HTML from {url}");
                using var client = new HttpClient();
                string html = await client.GetStringAsync(url);
                Logger.Info($"[HtmlFetcher] Successfully fetched HTML");
                return html;
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"[HtmlFetcher] Web request failed: {ex.Message}");
                return "";
            }
            catch (Exception ex)
            {
                Logger.Error($"[HtmlFetcher] Unexpected error: {ex.Message}");
                return "";
            }
        }
    }
}
