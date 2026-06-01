using CourseEventsApi.DTOs;
using CourseEventsApi.Models;
using CourseEventsApi.Shared.Enums;

namespace CourseEventsApi.Services
{
    public class EventNormalizationService
    {
        public Event Normalize(ExternalEventDTO dto)
        {
            return new Event
            {
                Id = Guid.NewGuid(),

                Title = dto.Title ?? "No title",
                Description = dto.Description ?? dto.Title ?? "No description",
                Content = dto.Content,

                Category = EventCategory.Other,

                Country = dto.Country ?? "",
                Region = dto.Region ?? "",
                City = dto.City ?? "",

                Latitude = null,
                Longitude = null,

                PublishedAt = ParseDate(dto.PublishedAt),

                PriorityScore = 0,
                IsAnalyzed = false,

                Sources = new List<EventSource>
            {
                new EventSource
                {
                    Id = Guid.NewGuid(),
                    SourceName = dto.SourceName ?? "",
                    Url = dto.Url,
                    ImageUrl = dto.ImageUrl,
                    ApiId = dto.ApiId,
                    AuthorName = dto.AuthorName,
                    PublishedAt = ParseDateNullable(dto.PublishedAt)
                }
            }
            };
        }

        private DateTimeOffset ParseDate(string date)
        {
            if (DateTimeOffset.TryParse(date, out var result))
                return result.ToUniversalTime();

            return DateTimeOffset.UtcNow;
        }

        private DateTimeOffset? ParseDateNullable(string date)
        {
            if (DateTimeOffset.TryParse(date, out var result))
                return result.ToUniversalTime();

            return null;
        }
    }
}
