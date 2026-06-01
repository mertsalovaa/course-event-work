using CourseEventsApi.Shared.Enums;

namespace CourseEventsApi.DTOs
{
    public class EventResponseDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int PriorityScore { get; set; }

        public List<string> Sources { get; set; }
    }
}
