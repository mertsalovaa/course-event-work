namespace CourseEventsApi.DTOs
{
    public class EventDetailDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public string Country { get; set; } = "";
        public string Region { get; set; } = "";
        public string City { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int PriorityScore { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
        public List<EventSourceDTO> Sources { get; set; } = [];
    }

    public class EventSourceDTO
    {
        public string SourceName { get; set; } = "";
        public string Url { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? AuthorName { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
    }
}
