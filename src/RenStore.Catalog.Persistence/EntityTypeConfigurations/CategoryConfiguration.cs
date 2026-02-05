using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
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
                .Property(x => x.Description)
                .HasColumnName("category_description")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500)
                .IsRequired(false);
        
            builder
                .Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasColumnType("boolean")
                .HasDefaultValue("false")
                .IsRequired();
            
            builder
                .Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean")
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
                .HasMany(x => x.SubCategories)
                .WithOne()
                .HasForeignKey(x => x.CategoryId);
            
            builder
                .HasIndex(x => x.NormalizedName)
                .IsUnique();
            
            builder
                .HasIndex(x => x.NormalizedNameRu)
                .IsUnique();
        }
    }
}