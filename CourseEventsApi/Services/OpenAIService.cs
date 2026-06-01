using CourseEventsApi.Models.Result;
using CourseEventsApi.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using CourseEventsApi.Shared.Enums;
using CourseEventsApi.DTOs;
using CourseEventsApi.Models;

namespace CourseEventsApi.Services
{
    public class OpenAIService : IAIAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenAIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        //        public async Task<EventAnalysisResult> AnalyzeAsync(string text)
        //        {
        //            var categories = string.Join(", ", Enum.GetNames<EventCategory>());
        //            var apiKey = Environment.GetEnvironmentVariable("CourseOpenAI__ApiKey");

        //            var requestBody = new
        //            {
        //                model = "gpt-4.1-mini",
        //                messages = new[]
        //    {
        //        new
        //        {
        //            role = "system",
        //            content = @$"
        //You are an AI system for classifying news events.

        //Return ONLY valid JSON.

        //Category must be EXACTLY one of:
        //{categories}

        //Do not invent new categories.
        //Do not return explanations.
        //"
        //        },
        //        new
        //        {
        //            role = "user",
        //            content = $@"
        //Classify this event and return JSON in this format:
        //{{
        //  ""category"": ""string"",
        //  ""summary"": ""string"",
        //  ""priorityScore"": number (1-100)
        //}}

        //Event:
        //{text}
        //"
        //        }
        //    },
        //                temperature = 0.2
        //            };
        //            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");

        //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        //            request.Content = new StringContent(
        //                JsonSerializer.Serialize(requestBody),
        //                Encoding.UTF8,
        //                "application/json"
        //            );

        //            var response = await _httpClient.SendAsync(request);

        //            var json = await response.Content.ReadAsStringAsync();

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                throw new Exception($"OpenAI error: {json}");
        //            }

        //            using var doc = JsonDocument.Parse(json);

        //            var content = doc
        //                .RootElement
        //                .GetProperty("choices")[0]
        //                .GetProperty("message")
        //                .GetProperty("content")
        //                .GetString();

        //            content = CleanJson(content);

        //            var result = JsonSerializer.Deserialize<EventAnalysisResult>(
        //                content,
        //                new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                }
        //            );
        //            if (result == null)
        //            {
        //                throw new Exception("Failed to parse AI response");
        //            }

        //            return result;
        //        }

        private string CleanJson(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return content;

            content = content.Trim();

            // якщо приходить ```json ... ```
            if (content.StartsWith("```"))
            {
                var firstLineEnd = content.IndexOf('\n');
                content = content.Substring(firstLineEnd + 1);
                content = content.TrimEnd('`').Trim();
            }

            // якщо обгорнуто в "..."
            if (content.StartsWith("\""))
            {
                content = JsonSerializer.Deserialize<string>(content);
            }

            return content;
        }

        public async Task<GroupAnalysisResult> AnalyzeGroupAsync(List<Event> events)
        {
            var combined = string.Join("\n\n", events.Select(e =>
                $"Title: {e.Title}\nDescription: {e.Description}"
            ));

            var categories = string.Join(", ", Enum.GetNames<EventCategory>());

            var prompt = $@"
    You are an AI that merges multiple news into ONE event.
    
    Return ONLY JSON:
    {{
      ""title"": ""string"",
      ""summary"": ""string"",
      ""category"": ""string"",
      ""country"": ""string"",
      ""region"": ""string"",
      ""city"": ""string"",
      ""latitude"": number or null,
      ""longitude"": number or null,
      ""priorityScore"": number (1-100)
    }}

    STRICT RULES:
    - Return ONLY valid JSON, no extra text
    - title, summary, country, region, city MUST be in Ukrainian language
    - Be as accurate and precise as possible — do not guess or hallucinate facts
    - summary must be detailed, informative and based ONLY on provided news
    - priorityScore must reflect real-world importance (wars/disasters = high, local news = low)

    Category must be one of (return in English):
    {categories}

    For latitude and longitude:
    - If city is known, return its precise coordinates
    - If only region is known, return region center coordinates
    - If only country is known, return country capital coordinates
    - If location is completely unknown, return null for both

    Events:
    {combined}
    ";

            var content = await SendToOpenAI(prompt);
            content = CleanJson(content);
            var result = JsonSerializer.Deserialize<GroupAnalysisResult>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (result == null)
                throw new Exception("AI parsing failed");

            return result;
        }

        private async Task<string> SendToOpenAI(string prompt)
        {
            var apiKey = Environment.GetEnvironmentVariable("CourseOpenAI__ApiKey");

            var requestBody = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
            new
            {
                role = "system",
                content = "You are an AI that analyzes and aggregates news events. Return ONLY valid JSON."
            },
            new
            {
                role = "user",
                content = prompt
            }
        },
                temperature = 0.2
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/chat/completions"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI error: {json}");
            }

            using var doc = JsonDocument.Parse(json);

            var content = doc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // 🔥 якщо AI повернув JSON як string у string
            if (content.StartsWith("\""))
            {
                content = JsonSerializer.Deserialize<string>(content);
            }

            return content;
        }

        public async Task<List<double>> GetEmbeddingAsync(string text)
        {
            var apiKey = Environment.GetEnvironmentVariable("CourseOpenAI__ApiKey");

            var requestBody = new
            {
                model = "text-embedding-3-small",
                input = text
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/embeddings");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI embeddings error: {json}");
            }

            using var doc = JsonDocument.Parse(json);

            var embedding = doc
                .RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(x => x.GetDouble())
                .ToList();

            return embedding;
        }
    }
}
