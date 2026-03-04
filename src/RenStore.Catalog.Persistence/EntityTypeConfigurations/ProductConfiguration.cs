using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class ProductConfiguration
    : IEntityTypeConfiguration<ProductReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder
            .ToTable("products");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .Property(p => p.Id)
            .HasColumnName("id");
        
        builder
            .Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion(
                v => ProductStatusConversion.ToDatabase(v),
                v => ProductStatusConversion.FromDatabase(v))
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
            
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);

        builder
            .Property(p => p.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();
        
        builder
            .Property(p => p.SubCategoryId)
            .HasColumnName("sub_category_id")
            .IsRequired();
    }
}