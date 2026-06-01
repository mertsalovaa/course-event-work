using CourseEventsApi.Data;
using CourseEventsApi.Models;
using CourseEventsApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseEventsApi.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IDbContextFactory<AppDbContext> _factory;

        public EventRepository(IDbContextFactory<AppDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            await using var db = await _factory.CreateDbContextAsync();
            return await db.Events.ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            await using var db = await _factory.CreateDbContextAsync();
            return await db.Events.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Event entity)
        {
            await using var db = await _factory.CreateDbContextAsync();
            await db.Events.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Event> events)
        {
            await using var db = await _factory.CreateDbContextAsync();
            await db.Events.AddRangeAsync(events);
            await db.SaveChangesAsync();
        }

        public async Task<Event?> GetByIdWithSourcesAsync(Guid id)
        {
            await using var db = await _factory.CreateDbContextAsync();
            return await db.Events
                .Include(e => e.Sources)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            // no-op: кожен метод тепер зберігає сам
        }

        public async Task<List<Event>> GetAllWithSourcesAsync()
        {
            await using var db = await _factory.CreateDbContextAsync();
            return await db.Events
                .Include(e => e.Sources)
                .ToListAsync();
        }

        public async Task<Event?> FindSimilarAsync(List<double> embedding, double threshold = 0.85)
        {
            await using var db = await _factory.CreateDbContextAsync();

            var events = await db.Events
                .Include(e => e.Sources)
                .Where(e => e.Embedding != null)
                .ToListAsync();

            foreach (var e in events)
            {
                var sim = CosineSimilarity(embedding, e.Embedding);
                if (sim > threshold)
                    return e;
            }

            return null;
        }

        private double CosineSimilarity(List<double> v1, List<double> v2)
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
