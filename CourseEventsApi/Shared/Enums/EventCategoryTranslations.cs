namespace CourseEventsApi.Shared.Enums
{
    public static class EventCategoryTranslations
    {
        public static readonly Dictionary<EventCategory, string> Ukrainian = new()
    {
        { EventCategory.Politics, "Політика" },
        { EventCategory.Economy, "Економіка" },
        { EventCategory.Technology, "Технології" },
        { EventCategory.Science, "Наука" },
        { EventCategory.Health, "Здоров'я" },
        { EventCategory.Environment, "Екологія" },
        { EventCategory.Disaster, "Катастрофа" },
        { EventCategory.Sports, "Спорт" },
        { EventCategory.Entertainment, "Розваги" },
        { EventCategory.Security, "Безпека" },
        { EventCategory.Military, "Військове" },
        { EventCategory.Finance, "Фінанси" },
        { EventCategory.Other, "Інше" }
    };
    }

}
