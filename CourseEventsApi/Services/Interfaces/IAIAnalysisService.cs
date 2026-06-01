using CourseEventsApi.DTOs;
using CourseEventsApi.Models;
using CourseEventsApi.Models.Result;

namespace CourseEventsApi.Services.Interfaces
{
    public interface IAIAnalysisService
    {
        //Task<EventAnalysisResult> AnalyzeAsync(string entity);
        Task<GroupAnalysisResult> AnalyzeGroupAsync(List<Event> events);
        Task<List<double>> GetEmbeddingAsync(string text);
    }
}
