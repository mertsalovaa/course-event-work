using CourseEventsApi.DTOs;
using CourseEventsApi.Models;
using CourseEventsApi.Repositories.Interfaces;
using CourseEventsApi.Services.Interfaces;
using CourseEventsApi.Shared.Enums;

namespace CourseEventsApi.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EventResponseDTO>> GetAllAsync()
        {
            var events = await _repository.GetAllWithSourcesAsync();

            return events.Select(e => new EventResponseDTO
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Category = EventCategoryTranslations.Ukrainian[e.Category],

                Country = e.Country,
                Region = e.Region,
                City = e.City,

                PriorityScore = e.PriorityScore,

                Latitude = e.Latitude,
                Longitude = e.Longitude,

                Sources = e.Sources.Select(s => s.Url).ToList()
            }).ToList();
        }

        public async Task<EventDetailDTO?> GetByIdAsync(Guid id)
        {
            var ev = await _repository.GetByIdWithSourcesAsync(id);
            if (ev == null) return null;

            return new EventDetailDTO
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Category = EventCategoryTranslations.Ukrainian[ev.Category],
                Country = ev.Country,
                Region = ev.Region,
                City = ev.City,
                Latitude = ev.Latitude,
                Longitude = ev.Longitude,
                PriorityScore = ev.PriorityScore,
                PublishedAt = ev.PublishedAt,
                Sources = ev.Sources.Select(s => new EventSourceDTO
                {
                    SourceName = s.SourceName,
                    Url = s.Url,
                    ImageUrl = s.ImageUrl,
                    AuthorName = s.AuthorName,
                    PublishedAt = (DateTimeOffset)s.PublishedAt
                }).ToList()
            };
        }

        public async Task<Event> CreateAsync(ExternalEventDTO dto)
        {
            var entity = new Event
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Category = Enum.TryParse<EventCategory>(dto.Category, out var category) ? category : EventCategory.Other,
                Country = "",
                Region = "",
                City = "",
                PublishedAt = DateTime.Parse(dto.PublishedAt).ToUniversalTime(),
                PriorityScore = 0,
                IsAnalyzed = false
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return entity;
        }

        //public async Task<Event> UpsertAsync(ExternalEventDTO dto)
        //{
        //    var eventText = $"{dto.Title}. {dto.Description}";

        //    var existingEvent = await _repository.FindBySimilarityAsync(eventText);

        //    var source = new EventSource
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = dto.Title,
        //        Url = dto.Url,
        //        SourceName = dto.SourceName,
        //        Content = dto.Content,
        //        PublishedAt = DateTime.Parse(dto.PublishedAt)
        //    };

        //    if (existingEvent != null)
        //    {
        //        existingEvent.Sources.Add(source);

        //        await _repository.UpdateAsync(existingEvent);
        //        await _repository.SaveChangesAsync();

        //        return existingEvent;
        //    }

        //    var newEvent = new Event
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = dto.Title,
        //        Description = dto.Description,
        //        Category = Enum.TryParse<EventCategory>(dto.Category, out var c)
        //            ? c
        //            : EventCategory.Other,
        //        Country = "",
        //        Region = "",
        //        City = "",
        //        PublishedAt = DateTime.Parse(dto.PublishedAt),
        //        PriorityScore = 0,
        //        IsAnalyzed = false,
        //        Sources = new List<EventSource> { source }
        //    };

        //    await _repository.AddAsync(newEvent);
        //    await _repository.SaveChangesAsync();

        //    return newEvent;
        //}
    
    }
}