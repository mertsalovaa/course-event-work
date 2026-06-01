namespace CourseEventsApi.Models.Result
{
    public class GroupAnalysisResult
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int PriorityScore { get; set; }
    }
}
