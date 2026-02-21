using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductPriceHistoryConfiguration 
    : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder
            .ToTable("price_history");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .OwnsOne(x => x.Price, price =>
            {
                price
                    .Property(x => x.Amount)
                    .HasColumnName("price")
                    .IsRequired();
                
                price 
                    .Property(x => x.Currency)
                    .HasColumnName("currency")
                    .IsRequired();
            });
        
        builder
            .Property(x => x.ValidFrom)
            .HasColumnName("valid_from")
            .IsRequired();
        
        builder
            .Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.DeactivatedAt)
            .HasColumnName("deactivated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.SizeId)
            .HasColumnName("size_id");

        /*builder
            .HasOne(x => x.SizeId)
            .WithMany()
            .HasForeignKey(x => x.S)
            .HasConstraintName("size_id");*/
    }
}