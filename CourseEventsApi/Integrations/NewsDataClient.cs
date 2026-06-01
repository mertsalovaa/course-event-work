using CourseEventsApi.DTOs;
using CourseEventsApi.Integrations.Interfaces;
using System.Text.Json;

namespace CourseEventsApi.Integrations
{
    public class NewsDataClient : INewsApiClient
    {
        private readonly HttpClient _httpClient;

        public NewsDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalEventDTO>> GetLatestNewsAsync()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("NewsData__ApiKey");

                var url = $"https://newsdata.io/api/1/latest?apikey={apiKey}&language=en";

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"GNews error: {(int)response.StatusCode}"
                    );
                    return [];
                }

                var data = JsonSerializer.Deserialize<NewsDataResponse>(json);
                if (data == null)
                {
                    throw new Exception($"NewsData value-data is NULL");
                }
                return data.results.Select(x => new ExternalEventDTO
                {
                    ApiId = x.article_id,
                    Title = x.title,
                    Description = x.description ?? x.title,
                    Url = x.link,
                    ImageUrl = x.image_url ?? "",
                    PublishedAt = x.pubDate,
                    SourceName = x.source_id
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NewsData failed: {ex.Message}");
                return [];
            }
        }
    }
}
