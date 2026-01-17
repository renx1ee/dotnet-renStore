/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductClothSizeConfiguration : IEntityTypeConfiguration<ProductClothSizeEntity>
{
    public void Configure(EntityTypeBuilder<ProductClothSizeEntity> builder)
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
            .Property(s => s.Amount)
            .HasColumnName("amount")
            .IsRequired();
        
        builder
            .HasOne(s => s.ProductCloth)
            .WithMany(c => c.ClothSizes)
            .HasForeignKey(s => s.ProductClothId)
            .HasConstraintName("product_cloth_id");

        builder
            .Property(s => s.ProductClothId)
            .HasColumnName("product_cloth_id");
    }
}*/