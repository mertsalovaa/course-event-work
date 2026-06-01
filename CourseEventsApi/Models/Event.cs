using CourseEventsApi.Shared.Enums;

namespace CourseEventsApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string? Content { get; set; }

        public EventCategory Category { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        public int PriorityScore { get; set; }

        public bool IsAnalyzed { get; set; }

        public List<double>? Embedding { get; set; }

        public List<EventSource> Sources { get; set; } = new();
    }
}
