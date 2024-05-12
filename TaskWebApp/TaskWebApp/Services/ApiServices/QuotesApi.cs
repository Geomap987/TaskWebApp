using System.Text.Json;

namespace TaskWebApp.Services.ApiServices
{
    public class QuotesApi
    {
        private HttpClient _httpClient;
        public QuotesApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(string QuoteText, string QuoteAuthor)> GetRandomQuote()
        {
            var quoteJson = await _httpClient.GetStringAsync("random");
            using var doc = JsonDocument.Parse(quoteJson);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
            {
                var firstElement = root[0];

                var quoteText = firstElement.GetProperty("q").GetString();
                var quoteAuthor = firstElement.GetProperty("a").GetString();

                return (quoteText, quoteAuthor);
            }

            throw new InvalidOperationException("No quotes were returned");
        }
    }
}
