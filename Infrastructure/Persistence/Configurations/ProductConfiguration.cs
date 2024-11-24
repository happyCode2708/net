using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Models;

namespace MyApi.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasIndex(p => p.UniqueId).IsUnique();
        }
    }
}