using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;

namespace MyApi.application.common.interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Overwrite the SaveChanges
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}