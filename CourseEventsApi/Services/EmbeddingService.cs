using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace CourseEventsApi.Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _http;

        public EmbeddingService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<double>> GenerateAsync(string text)
        {
            var apiKey = Environment.GetEnvironmentVariable("CourseOpenAI__ApiKey");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/embeddings");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    model = "text-embedding-3-small",
                    input = text
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(json);

            using var doc = JsonDocument.Parse(json);

            var arr = doc.RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(x => x.GetDouble())
                .ToList();

            return arr;
        }

        public double CosineSimilarity(List<double> v1, List<double> v2)
        {
            double dot = 0, mag1 = 0, mag2 = 0;

            for (int i = 0; i < v1.Count; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }

            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
