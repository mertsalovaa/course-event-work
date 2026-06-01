namespace CourseEventsApi.Models
{
    public static class VectorUtils
    {
        public static double CosineSimilarity(List<double> v1, List<double> v2)
        {
            if (v1.Count != v2.Count)
                throw new ArgumentException("Vectors must be same length");

            double dot = 0;
            double mag1 = 0;
            double mag2 = 0;

            for (int i = 0; i < v1.Count; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += Math.Pow(v1[i], 2);
                mag2 += Math.Pow(v2[i], 2);
            }

            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
