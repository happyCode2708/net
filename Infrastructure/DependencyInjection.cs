using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Services;
using MyApi.Infrastructure.Persistence;
using MyApi.Infrastructure.Services;
using MediatR;
using MyApi.Application.Common.Configs;


namespace MyApi.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddApplicationContext(configuration);
    services.AddAiServices();
    services.AddEntitiesServices();
    services.AddBaseServices();
    services.BindConfig(configuration);
    services.BindMapper();

    return services;
  }

  private static IServiceCollection AddApplicationContext(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

    var connectionString = string.Format(
        configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
        dbHost, dbPort, dbName, dbUser, dbPassword);

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    return services;
  }

  private static IServiceCollection AddAiServices(
      this IServiceCollection services)
  {
    services.AddSingleton<IPromptBuilderService, PromptBuilderService>();
    services.AddSingleton<IGenerativeServices, GenerativeServices>();
    services.AddSingleton<IGeminiServices, GeminiServices>();
    return services;
  }


  private static IServiceCollection AddEntitiesServices(
      this IServiceCollection services)
  {
    services.AddScoped<IProductServices, ProductServices>();
    services.AddScoped<IImageServices, ImageServices>();
    return services;
  }

  private static IServiceCollection AddBaseServices(
      this IServiceCollection services)
  {
    services.AddHttpClient();
    services.AddCors(options =>
    {
      options.AddPolicy("AllowNextJsApp", policy =>
      {
        policy.WithOrigins("http://localhost:3000") // Replace with your Next.js app URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // if you need to send cookies or auth headers
      });
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddControllers().AddNewtonsoftJson();

    services.AddMediatR(typeof(Program).Assembly);

    return services;
  }

  private static IServiceCollection BindConfig(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<StorageConfig>(configuration.GetSection("StorageConfig"));
    services.Configure<CredentialConfig>(configuration.GetSection("Credentials"));
    return services;
  }

  private static IServiceCollection BindMapper(
      this IServiceCollection services)
  {
    services.AddAutoMapper(typeof(Program).Assembly);
    return services;
  }
}
