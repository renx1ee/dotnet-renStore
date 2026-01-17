/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder
            .ToTable("sub_categories");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("sub_category_id");
        
        builder
            .Property(x => x.Name)
            .HasColumnName("sub_category_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedName)
            .HasColumnName("normalized_sub_category_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedName)
            .IsUnique();
        
        builder
            .Property(x => x.NameRu)
            .HasColumnName("sub_category_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_sub_category_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedNameRu)
            .IsUnique();
        
        builder
            .Property(x => x.Description)
            .HasColumnName("sub_category_description")
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
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder
            .Property(x => x.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}*/