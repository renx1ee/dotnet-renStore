using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class PickupPointConfiguration : IEntityTypeConfiguration<PickupPoint>
{
    public void Configure(EntityTypeBuilder<PickupPoint> builder)
    {
        builder
            .ToTable("pickup_points");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("pickup_point_id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValueSql("false")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .IsRequired();
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("delete_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .Property(x => x.AddressId)
            .HasColumnName("address_id")
            .IsRequired();
        
        builder
            .HasOne(typeof(Address), "_address")
            .WithMany()
            .HasForeignKey("AddressId")
            .IsRequired(false);
    }
}