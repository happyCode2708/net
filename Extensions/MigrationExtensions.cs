using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Persistence;

namespace Extensions
{
  public static class MigrationExtensions
  {
    public static void ApplyMigrations(this WebApplication app)
    {
      using (var scope = app.Services.CreateScope())
      {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
      }
    }
  }
}