namespace CourseEventsApi.DTOs
{
    public class DisasterEventDTO
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string Severity { get; set; }
        public DateTime? Date { get; set; }
        public string Url { get; set; }
    }
}
