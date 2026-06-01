namespace CourseEventsApi.Models.Result
{
    public class EventAnalysisResult
    {
        public string Title { get; set; }          // 🔥 нове
        public string Description { get; set; }    // 🔥 нове

        public string Category { get; set; }
        public int PriorityScore { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class EmbeddingResponse
    {
        public List<double> Vector { get; set; } = new();
    }
}
