namespace CourseEventsApi.Models
{
    public class EventSource
    {
        public Guid Id { get; set; }

        public string SourceName { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }
        public string? ApiId { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }

        public string? AuthorName { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
