using CourseEventsApi.Shared.Enums;

namespace CourseEventsApi.Shared.Extensions
{
    public static class NewsCategoryExtensions
    {
        public static string ToApiOption(this NewsApiCategoriesEnum category)
        {
            return category.ToString().ToLower();
        }
    }
}
