using CourseEventsApi.DTOs;
using CourseEventsApi.Integrations.Interfaces;
using System.Text.Json;

namespace CourseEventsApi.Integrations
{
    public class GNewsApiClient : INewsApiClient
    {
        private readonly HttpClient _httpClient;

        public GNewsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalEventDTO>> GetLatestNewsAsync()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("GNews__ApiKey");
                var url = "https://gnews.io/api/v4/top-headlines";

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.Add("X-Api-Key", apiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"GNews error: {(int)response.StatusCode}"
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
                        ApiId = article.GetProperty("id").GetString() ?? "",
                        Title = article.GetProperty("title").GetString() ?? "",
                        Description = article.GetProperty("description").GetString() ?? "",
                        SourceName = article.GetProperty("source").GetProperty("name").GetString() ?? "",
                        Url = article.GetProperty("url").GetString() ?? "",
                        ImageUrl = article.GetProperty("image").GetString() ?? "",
                        AuthorName = article.GetProperty("source").GetProperty("name").GetString() ?? "",
                        PublishedAt = article.GetProperty("publishedAt").GetString() ?? "",
                        Content = article.GetProperty("content").GetString() ?? "",
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GNews failed: {ex.Message}");
                return [];
            }
        }
    }
}
