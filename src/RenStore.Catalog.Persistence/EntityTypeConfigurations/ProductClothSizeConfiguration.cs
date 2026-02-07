/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductClothSizeConfiguration : IEntityTypeConfiguration<ProductClothSize>
{
    public void Configure(EntityTypeBuilder<ProductClothSize> builder)
    {
        builder
            .ToTable("product_cloth_sizes");
        
        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasColumnName("cloth_size_id");
        
        builder
            .Property(s => s.ClothSize)
            .HasColumnName("cloth_size")
            .IsRequired();
        
        builder
            .Property(s => s.InStock)
            .HasColumnName("amount")
            .IsRequired();
        
        builder
            .HasOne(s => s.ProductCloth)
            .WithMany(c => c.ClothSizes)
            .HasForeignKey(s => s.ProductVariantId)
            .HasConstraintName("product_cloth_id");

        builder
            .Property(s => s.ProductVariantId)
            .HasColumnName("product_cloth_id");
    }
}*/