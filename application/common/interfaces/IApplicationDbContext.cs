using Microsoft.EntityFrameworkCore;
using MyApi.Domain.Models;

namespace MyApi.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ExtractSession> ExtractSessions { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}