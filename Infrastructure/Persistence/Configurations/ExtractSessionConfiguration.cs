using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Models;

namespace MyApi.Infrastructure.Persistence.Configurations
{
    public class ExtractSessionConfiguration : IEntityTypeConfiguration<ExtractSession>
    {
        public void Configure(EntityTypeBuilder<ExtractSession> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.SourceType).IsRequired();
            builder.Property(e => e.ExtractorVersion).IsRequired();
            builder.Property(e => e.ErrorMessage).IsRequired(false);
            builder.Property(e => e.RawExtractData).IsRequired(false);
            builder.Property(e => e.ExtractedData).IsRequired(false);
            builder.Property(e => e.ValidatedExtractedData).IsRequired(false);

            builder.HasOne(e => e.ProductItem).WithMany(p => p.ExtractSessions).HasForeignKey(e => e.ProductId);

            builder.HasIndex(e => new { e.ProductId, e.SourceType, e.CreatedAt });
        }
    }
}