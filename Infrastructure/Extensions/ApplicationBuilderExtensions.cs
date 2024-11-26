using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyApi.Infrastructure.Persistence;

namespace Infrastructure.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseDbReSeed(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.CreateScope())
      {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ReseedDb(context);
      }
      return app;
    }

    public static IApplicationBuilder UseStaticFilesByAssetPath(this IApplicationBuilder app, IConfiguration configuration)
    {
      using (var scope = app.ApplicationServices.CreateScope())
      {

        var assetPath = Environment.GetEnvironmentVariable("ASSET_PATH") ?? configuration["StorageConfig:DefaultAssetPath"];
        var assetPathRequest = configuration["StorageConfig:AssetPathRequest"];

        var staticPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);

        if (!Directory.Exists(staticPath))
        {
          Directory.CreateDirectory(staticPath);
        }

        app.UseStaticFiles(new StaticFileOptions
        {
          FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), assetPath)),
          RequestPath = assetPathRequest
        });
      }
      return app;
    }

    private static void ReseedDb(ApplicationDbContext context)
    {
      try
      {
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
        Console.WriteLine($"Error during database initialization: {ex.Message}");
      }
    }
  }
}