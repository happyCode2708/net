using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Models;

namespace MyApi.Infrastructure.Persistence.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => new { pi.ProductId, pi.ImageId });
            builder.HasOne(pi => pi.Product).WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId);
            builder.HasOne(pi => pi.Image).WithMany(i => i.ProductImages).HasForeignKey(pi => pi.ImageId);
        }
    }
}