/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductClothConfiguration : IEntityTypeConfiguration<ProductCloth>
{
    public void Configure(EntityTypeBuilder<ProductCloth> builder)
    {
        builder
            .ToTable("product_clothes");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("product_cloth_id");
        
        builder
            .Property(c => c.Gender)
            .HasColumnName("gender")
            .IsRequired(false);
        
        builder
            .Property(c => c.Season)
            .HasColumnName("season")
            .IsRequired(false);
        
        builder
            .Property(c => c.Neckline)
            .HasColumnName("neckline")
            .IsRequired(false);
        
        builder
            .Property(c => c.TheCut)
            .HasColumnName("the_cut")
            .IsRequired(false);
        
        builder
            .HasOne(c => c.Product)
            .WithOne(p => p.ProductCloth)
            .HasForeignKey<ProductCloth>(c => c.ProductId)
            .HasConstraintName("product_id");

        builder
            .Property(c => c.ProductId)
            .HasColumnName("product_id");

        builder
            .HasMany(c => c.ClothSizes)
            .WithOne(s => s.ProductCloth)
            .HasForeignKey(s => s.ProductVariantId);
    }
}*/