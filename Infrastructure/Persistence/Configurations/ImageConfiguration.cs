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
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);
            builder
                .Property(i => i.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(1, 1)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            builder.HasIndex(i => i.UniqueId).IsUnique();
        }
    }
}