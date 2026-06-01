using CourseEventsApi.DTOs;
using CourseEventsApi.Hubs;
using CourseEventsApi.Integrations;
using CourseEventsApi.Integrations.Interfaces;
using CourseEventsApi.Models;
using CourseEventsApi.Repositories.Interfaces;
using CourseEventsApi.Services.Interfaces;
using CourseEventsApi.Shared.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CourseEventsApi.Services
{
    public class EventProcessingService : IEventProcessingService
    {
        private readonly NewsApiClient _newsApi;
        private readonly GNewsApiClient _gNewsApi;
        private readonly EventNormalizationService _normalizer;
        private readonly IAIAnalysisService _ai;
        private readonly NewsDataClient _newsData;
        private readonly EmbeddingService _embedding;
        private readonly DedupService _dedup;
        private readonly IEventRepository _repo;
        private readonly IHubContext<DashboardHub> _hub;


        public EventProcessingService(
            NewsApiClient newsApi,
            GNewsApiClient gNewsApi,
            EventNormalizationService normalizer,
            NewsDataClient newsData,
            IAIAnalysisService ai,
            EmbeddingService embedding,
            DedupService dedup,
            IEventRepository repo,
            IHubContext<DashboardHub> hub
            )
        {
            _newsApi = newsApi;
            _gNewsApi = gNewsApi;
            _normalizer = normalizer;
            _newsData = newsData;
            _embedding = embedding;
            _dedup = dedup;
            _ai = ai;
            _repo = repo;
            _hub = hub;

        }

        public async Task<int> ImportAsync()
        {
            // 1. Отримання
            var tasks = await Task.WhenAll(
                _newsApi.GetLatestNewsAsync(),
                _gNewsApi.GetLatestNewsAsync(),
                _newsData.GetLatestNewsAsync()
            );
            var all = tasks.SelectMany(x => x).ToList();

            // 2. Normalize
            var normalized = all
                .Select(x => _normalizer.Normalize(x))
                .ToList();

            // 3. Embeddings для сирих подій
            //foreach (var e in normalized)
            //{
            //    var text = $"{e.Title}. {e.Description}";
            //    e.Embedding = await _embedding.GenerateAsync(text);
            //}

            await Parallel.ForEachAsync(normalized, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (e, _) =>
            {
                var text = $"{e.Title}. {e.Description}";
                e.Embedding = await _embedding.GenerateAsync(text);
            });

            // 4. Кластеризація (всередині batch)
            var clusters = _dedup.Cluster(normalized);

            // 5. Обробка кожного кластера
            var newEvents = new ConcurrentBag<Event>();

            await Parallel.ForEachAsync(clusters, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (cluster, _) =>
            {
                var combinedText = string.Join("\n", cluster.Select(x => $"{x.Title}. {x.Description}"));
                var clusterEmbedding = await _embedding.GenerateAsync(combinedText);
                var existing = await _repo.FindSimilarAsync(clusterEmbedding);

                if (existing != null)
                {
                    foreach (var src in cluster.SelectMany(x => x.Sources))
                    {
                        var alreadyExists = existing.Sources.Any(s =>
                            s.Url == src.Url ||
                            (!string.IsNullOrEmpty(src.ApiId) && s.ApiId == src.ApiId));
                        if (alreadyExists) continue;

                        existing.Sources.Add(new EventSource
                        {
                            Id = Guid.NewGuid(),
                            SourceName = src.SourceName,
                            Url = src.Url,
                            ImageUrl = src.ImageUrl,
                            AuthorName = src.AuthorName,
                            PublishedAt = src.PublishedAt,
                            ApiId = src.ApiId
                        });
                    }
                    return;
                }

                var result = await _ai.AnalyzeGroupAsync(cluster);

                var entity = new Event
                {
                    Id = Guid.NewGuid(),
                    Title = result.Title,
                    Description = result.Summary,
                    Category = Enum.TryParse<EventCategory>(result.Category, true, out var cat) ? cat : EventCategory.Other,
                    Country = result.Country ?? "",
                    Region = result.Region ?? "",
                    City = result.City ?? "",
                    PublishedAt = cluster.Max(x => x.PublishedAt),
                    PriorityScore = result.PriorityScore,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    IsAnalyzed = true,
                    Embedding = clusterEmbedding,
                    Sources = cluster.SelectMany(x => x.Sources).ToList()
                };

                newEvents.Add(entity);
            });
            await _repo.AddRangeAsync(newEvents.ToList());
            var dashboard = await GetDashboardStatsAsync();
            await _hub.Clients.All.SendAsync(
    "dashboard:update",
    dashboard
);
            return newEvents.Count;

        }

        public async Task<List<ExternalEventDTO>> GetList()
        {
            var externalEvents = await _repo.GetAllAsync();
            return externalEvents.Select(x => new ExternalEventDTO
            {
                Title = x.Title,
                Description = x.Description,
                PublishedAt = x.PublishedAt.ToString(),
            }).ToList();
        }

        public async Task<List<ExternalEventDTO>> GetAllAsync()
        {
            var externalEvents = await _repo.GetAllAsync();

            var mappedEvents = externalEvents.Select(x => new ExternalEventDTO
            {
                Title = x.Title,
                Description = x.Description,
                PublishedAt = x.PublishedAt.ToString("O"),
                Country = x.Country,
            });


            return mappedEvents.ToList();
        }

        public async Task<Event?> FindSimilarEventAsync(string text)
        {
            var newEmbedding = await _ai.GetEmbeddingAsync(text);

            var events = await _repo.GetAllAsync();

            foreach (var existing in events)
            {
                if (existing.Embedding == null) continue;

                var similarity = VectorUtils.CosineSimilarity(newEmbedding, existing.Embedding);

                if (similarity > 0.85) // 🔥 поріг
                {
                    return existing;
                }
            }

            return null;
        }

        public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
        {
            var events = await _repo.GetAllAsync();
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var totalEvents = events.Count;

            var criticalEvents = events.Count(e =>
                e.PriorityScore > 80);

            var countriesCount = events
                .Where(e => !string.IsNullOrWhiteSpace(e.Country))
                .Select(e => e.Country)
                .Distinct()
                .Count();

            var averagePriorityScore = totalEvents > 0
                ? Math.Round(events.Average(e => e.PriorityScore), 1)
                : 0;

            var sourcesCount = events
                .SelectMany(e => e.Sources)
            .Count();

            var mostPopularCategory = events.GroupBy(e => e.Category)
              .OrderByDescending(g => g.Count())
              .Select(g => g.Key)
              .FirstOrDefault();

            var mostActiveCountry = events
                .Where(e => !string.IsNullOrWhiteSpace(e.Country))
                .GroupBy(e => e.Country)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Unknown";

            return new DashboardStatsDTO
            {
                TotalEvents = totalEvents,
                TodayEvents = events.Count(e =>
    e.PublishedAt >= last24Hours),
                CriticalEvents = criticalEvents,
                CountriesCount = countriesCount,
                AveragePriorityScore = averagePriorityScore,
                SourcesCount = sourcesCount,
                MostPopularCategory = EventCategoryTranslations.Ukrainian[mostPopularCategory],
                MostActiveCountry = mostActiveCountry
            };
        }
    }
}
