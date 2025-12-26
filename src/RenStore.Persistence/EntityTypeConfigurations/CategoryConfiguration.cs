using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder
            .ToTable("categories");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("category_id");
        
        builder
            .Property(x => x.Name)
            .HasColumnName("category_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedName)
            .HasColumnName("normalized_category_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedName)
            .IsUnique();
        
        builder
            .Property(x => x.NameRu)
            .HasColumnName("category_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_category_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedNameRu)
            .IsUnique();
        
        builder
            .Property(x => x.Description)
            .HasColumnName("category_description")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasColumnType("boolean")
            .HasDefaultValue("true")
            .IsRequired();
        
        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
        
        builder
            .HasMany(x => x.SubCategories)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId);
    }
}