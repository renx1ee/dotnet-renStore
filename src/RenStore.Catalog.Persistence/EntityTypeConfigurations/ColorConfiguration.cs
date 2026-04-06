using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder
            .ToTable("colors");

        builder
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();
        
        builder
            .Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(c => c.NormalizedName)
            .HasColumnName("normalized_name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(c => c.NameRu)
            .HasColumnName("name_ru")
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(c => c.NormalizedNameRu)
            .HasColumnName("normalized_name_ru")
            .HasMaxLength(50)
            .IsRequired();

        builder
            .OwnsOne(x => x.Code, code =>
            {
                code
                    .Property(c => c.Value)
                    .HasColumnName("color_code")
                    .HasMaxLength(7)
                    .IsRequired();

                code
                    .HasIndex(c => c.Value)
                    .HasDatabaseName("ux_colors_color_code")
                    .IsUnique();
            });
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValue("false")
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
            .HasIndex(c => c.NormalizedName)
            .IsUnique();
        builder
            .HasIndex(c => c.NameRu)
            .IsUnique();
    }
}