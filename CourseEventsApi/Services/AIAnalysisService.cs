using CourseEventsApi.Models.Result;
using CourseEventsApi.Models;
using CourseEventsApi.Services.Interfaces;

namespace CourseEventsApi.Services
{
    public class AIAnalysisService
    {
        public Task<EventAnalysisResult> AnalyzeAsync(Event entity)
        {
            var category = entity.Title.Contains("AI") ? "Artificial Intelligence" : "General";

            return Task.FromResult(new EventAnalysisResult
            {
                Category = category,
                //Summary = entity.Description.Length > 100
                //    ? entity.Description[..100]
                //    : entity.Description, 
                Description = entity.Description,
                PriorityScore = Random.Shared.Next(1, 100)
            });
        }
    }
}
