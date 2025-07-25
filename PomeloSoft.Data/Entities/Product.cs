using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PomeloSoft.Data.Entities;

public class Product : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageThumbnailUrl { get; set; } = null!;
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ImageThumbnailUrl)
            .HasMaxLength(1000);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Creator)
            .WithMany() 
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.NoAction); 

        builder.HasOne(p => p.LastModifier)
            .WithMany() 
            .HasForeignKey(p => p.LastModifierId)
            .OnDelete(DeleteBehavior.NoAction); 

        // Default value for IsActive
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
    }
} 