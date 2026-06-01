using CourseEventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseEventsApi.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task AddAsync(Event entity);
        Task SaveChangesAsync();
        Task<List<Event>> GetAllWithSourcesAsync();
        Task<Event?> FindSimilarAsync(List<double> embedding, double threshold = 0.85);
        Task AddRangeAsync(List<Event> events);
        Task<Event?> GetByIdWithSourcesAsync(Guid id);
    }
}
