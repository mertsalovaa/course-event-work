using CourseEventsApi.DTOs;
using CourseEventsApi.Models;

namespace CourseEventsApi.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<EventResponseDTO>> GetAllAsync();
        Task<EventDetailDTO?> GetByIdAsync(Guid id);
        Task<Event> CreateAsync(ExternalEventDTO dto);
    }
}
