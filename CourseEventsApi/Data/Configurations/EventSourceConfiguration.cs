using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CourseEventsApi.Models;
using System.Reflection.Emit;

namespace CourseEventsApi.Data.Configurations
{
    public class EventSourceConfiguration : IEntityTypeConfiguration<EventSource>
    {
        public void Configure(EntityTypeBuilder<EventSource> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Url)
            .IsRequired();

            builder.HasIndex(s => new { s.Url, s.EventId }).IsUnique();

            builder.Property(s => s.SourceName)
                .IsRequired();
        }
    }
}
