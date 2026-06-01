using CourseEventsApi.Models;

namespace CourseEventsApi.Services
{
    public class DedupService
    {
        public List<List<Event>> Cluster(List<Event> events)
        {
            var clusters = new List<List<Event>>();

            foreach (var e in events)
            {
                var found = clusters.FirstOrDefault(c =>
                    CosineSimilarity(c.First().Embedding, e.Embedding) > 0.85
                );

                if (found != null)
                    found.Add(e);
                else
                    clusters.Add(new List<Event> { e });
            }

            return clusters;
        }

        private double CosineSimilarity(List<double> v1, List<double> v2)
        {
            var dot = v1.Zip(v2, (a, b) => a * b).Sum();
            var mag1 = Math.Sqrt(v1.Sum(x => x * x));
            var mag2 = Math.Sqrt(v2.Sum(x => x * x));

            return dot / (mag1 * mag2);
        }
    }
}
