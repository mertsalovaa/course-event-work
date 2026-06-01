namespace CourseEventsApi.DTOs
{
    public class ExternalEventDTO
    {
        public string? ApiId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? Content { get; set; } = "";
        public string SourceName { get; set; } = "";
        public string? AuthorName { get; set; } = "";
        public string Url { get; set; } = "";
        public string? ImageUrl { get; set; } = "";
        public string PublishedAt { get; set; } = "";

        public string? Country { get; set; }
        public string? Category { get; set; }
        public string? Region { get; internal set; }
        public string? City { get; internal set; }
    }
}
