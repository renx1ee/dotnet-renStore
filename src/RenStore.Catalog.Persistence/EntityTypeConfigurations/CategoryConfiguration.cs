using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations
{
    public sealed class CategoryConfiguration 
        : IEntityTypeConfiguration<CategoryReadModel>
    {
        public void Configure(EntityTypeBuilder<CategoryReadModel> builder)
        {
            builder
                .ToTable("categories");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id");
        
            builder
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        
            builder
                .Property(x => x.NormalizedName)
                .HasColumnName("normalized_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        
            builder
                .Property(x => x.NameRu)
                .HasColumnName("name_ru")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        
            builder
                .Property(x => x.NormalizedNameRu)
                .HasColumnName("normalized_name_ru")
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
                .Property(x => x.UpdatedById)
                .HasColumnName("updated_by_id")
                .IsRequired();
            
            builder
                .Property(x => x.UpdatedByRole)
                .HasColumnName("updated_by_role")
                .HasMaxLength(20)
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
                .HasIndex(x => x.NormalizedName)
                .IsUnique();
            
            builder
                .HasIndex(x => x.NormalizedNameRu)
                .IsUnique();
        }
    }
}