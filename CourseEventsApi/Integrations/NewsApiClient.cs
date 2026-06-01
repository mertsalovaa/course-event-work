using CourseEventsApi.DTOs;
using CourseEventsApi.Integrations.Interfaces;
using System.Text.Json;

namespace CourseEventsApi.Integrations
{
    public class NewsApiClient : INewsApiClient
    {
        private readonly HttpClient _httpClient;
        
        public NewsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalEventDTO>> GetLatestNewsAsync()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("NewsApi__ApiKey");
                var url = "https://newsapi.org/v2/top-headlines?language=en";

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.Add("X-Api-Key", apiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"NewsApi error: {(int)response.StatusCode}"
                    );

                    return [];
                }

                var body = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();

                using var json = JsonDocument.Parse(body);

                var result = new List<ExternalEventDTO>();

                foreach (var article in json.RootElement.GetProperty("articles").EnumerateArray())
                {
                    result.Add(new ExternalEventDTO
                    {
                        Title = article.GetProperty("title").GetString() ?? "",
                        Description = article.GetProperty("description").GetString() ?? "",
                        SourceName = article.GetProperty("source").GetProperty("name").GetString() ?? "",
                        Url = article.GetProperty("url").GetString() ?? "",
                        ImageUrl = article.GetProperty("urlToImage").GetString() ?? "",
                        AuthorName = article.GetProperty("author").GetString() ?? "",
                        PublishedAt = article.GetProperty("publishedAt").GetString() ?? "",
                        Content = article.GetProperty("content").GetString() ?? "",
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NewsApi failed: {ex.Message}");
                return [];
            }
        }
    }
}
