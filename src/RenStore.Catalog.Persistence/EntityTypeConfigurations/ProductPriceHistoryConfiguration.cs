/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductPriceHistoryConfiguration : IEntityTypeConfiguration<ProductPriceHistoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductPriceHistoryEntity> builder)
    {
        builder
            .ToTable("product_price_histories");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("price_history_id");

        builder
            .Property(x => x.Price)
            .HasColumnName("price")
            .IsRequired();
        
        builder
            .Property(x => x.OldPrice)
            .HasColumnName("old_price")
            .IsRequired();
        
        builder
            .Property(x => x.DiscountPrice)
            .HasColumnName("discount_price")
            .IsRequired();
        
        builder
            .Property(x => x.DiscountPercent)
            .HasColumnName("discount_percent")
            .IsRequired();
        
        builder
            .Property(x => x.StartDate)
            .HasColumnName("start_date")
            .IsRequired();
        
        builder
            .Property(x => x.EndDate)
            .HasColumnName("end_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.ChangedBy)
            .HasColumnName("changed_by")
            .IsRequired();

        builder
            .HasOne(x => x.ProductVariant)
            .WithMany(x => x.PriceHistories)
            .HasForeignKey(x => x.ProductVariantId)
            .HasConstraintName("product_variant_id");

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}*/