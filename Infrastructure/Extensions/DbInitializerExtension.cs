using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Persistence;

namespace Infrastructure.Extensions
{
  public static class DbInitializerExtension
  {
    public static IApplicationBuilder UseDbReSeed(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.CreateScope())
      {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ReSeedDb(context);
      }
      return app;
    }

    private static void ReSeedDb(ApplicationDbContext context)
    {
      try
      {
        // Lấy max ID hiện tại
        var maxIdExtractSessions = context.ExtractSessions.Max(e => (int?)e.Id) ?? 0;
        var maxIdProducts = context.Products.Max(e => (int?)e.Id) ?? 0;
        var maxIdImages = context.Images.Max(e => (int?)e.Id) ?? 0;


        if (maxIdExtractSessions > 0)
        {
          context.Database.ExecuteSqlRaw(
              $"DBCC CHECKIDENT ('dbo.ExtractSessions', RESEED, {maxIdExtractSessions});");
        }
        if (maxIdProducts > 0)
        {
          context.Database.ExecuteSqlRaw(
              $"DBCC CHECKIDENT ('dbo.Products', RESEED, {maxIdProducts});");
        }
        if (maxIdImages > 0)
        {
          context.Database.ExecuteSqlRaw(
              $"DBCC CHECKIDENT ('dbo.Images', RESEED, {maxIdImages});");
        }
      }
      catch (Exception ex)
      {
        // Log error nếu cần
        Console.WriteLine($"Error during database initialization: {ex.Message}");
      }
    }
  }
}