using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class VariantPriceHistoryConfiguration 
    : IEntityTypeConfiguration<PriceHistoryReadModel>
{
    public void Configure(EntityTypeBuilder<PriceHistoryReadModel> builder)
    {
        builder
            .ToTable("price_history");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.Amount)
            .HasColumnName("price")
            .IsRequired();
                
        builder 
            .Property(x => x.Currency)
            .HasColumnName("currency")
            .HasConversion(
                p => VariantPriceHistoryConversion.CurrencyToDatabase(p),
                p => VariantPriceHistoryConversion.CurrencyFromDatabase(p))
            .IsRequired();
        
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

        builder
            .HasIndex(x => new { x.SizeId, x.Amount })
            .HasDatabaseName("ux_price_history_size_id_price");
    }
}