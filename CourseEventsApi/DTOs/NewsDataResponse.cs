namespace CourseEventsApi.DTOs
{
    public class NewsDataResponse
    {
        public List<NewsDataArticle> results { get; set; }
    }

    public class NewsDataArticle
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string image_url { get; set; }
        public string pubDate { get; set; }
        public string source_id { get; set; }
        public string article_id { get; set; }
    }
}
