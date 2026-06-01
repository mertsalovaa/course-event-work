using CourseEventsApi.DTOs;

namespace CourseEventsApi.Integrations.Interfaces
{
    public interface INewsApiClient
    {
        Task<List<ExternalEventDTO>> GetLatestNewsAsync();
    }
}
