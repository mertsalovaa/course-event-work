using CourseEventsApi.DTOs;

namespace CourseEventsApi.Services.Interfaces
{
    public interface IEventProcessingService
    {
        Task<int> ImportAsync();
        Task<List<ExternalEventDTO>> GetList();
        Task<List<ExternalEventDTO>> GetAllAsync();
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
    }
}
