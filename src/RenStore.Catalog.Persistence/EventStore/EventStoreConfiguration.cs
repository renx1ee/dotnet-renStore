using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RenStore.Catalog.Persistence.EventStore;

public sealed class EventStoreConfiguration
    : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder
            .ToTable("catalog_events");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(x => x.AggregateId)
            .HasColumnName("aggregate_id")
            .IsRequired();

        builder
            .Property(x => x.AggregateType)
            .HasColumnName("aggregate_type")
            .IsRequired();

        builder
            .Property(x => x.Version)
            .HasColumnName("version")
            .IsRequired();
        
        builder
            .Property(x => x.EventType)
            .HasColumnName("event_type")
            .IsRequired();
        
        /*builder
            .Property(x => x.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .IsRequired();*/
        
        builder
            .Property(x => x.Payload)
            .HasColumnName("payload")
            .HasColumnType("jsonb")
            .IsRequired(false);
        
        builder
            .Property(x => x.OccurredAtUtc)
            .HasColumnName("occurred_date")
            .IsRequired();

        builder
            .HasIndex(x => new { x.AggregateId, x.Version })
            .HasDatabaseName("ux_events_aggregate_id_version")
            .IsUnique();
    }
}