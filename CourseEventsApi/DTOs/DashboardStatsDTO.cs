namespace CourseEventsApi.DTOs
{
    public class DashboardStatsDTO
    {
        public int TotalEvents { get; set; }

        public int TodayEvents { get; set; }

        public int CriticalEvents { get; set; }

        public int CountriesCount { get; set; }

        public double AveragePriorityScore { get; set; }

        public int SourcesCount { get; set; }
    
        public string MostPopularCategory { get; set; } = string.Empty;

        public string MostActiveCountry { get; set; } = string.Empty;
    }
}
