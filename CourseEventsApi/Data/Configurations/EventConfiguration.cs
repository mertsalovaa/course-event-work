using CourseEventsApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourseEventsApi.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired();

            builder.Property(e => e.Description)
                .IsRequired();

            builder.Property(e => e.Category)
    .HasConversion<string>();

            builder.Property(e => e.Embedding)
     .HasColumnType("jsonb")
     .HasConversion(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<List<double>>(v, (JsonSerializerOptions)null)
    )
    .Metadata.SetValueComparer(
        new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<double>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        ));

            // 🔥 зв'язок з джерелами
            builder.HasMany(e => e.Sources)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
