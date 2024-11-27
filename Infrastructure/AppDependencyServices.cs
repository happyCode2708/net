using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Services;
using MyApi.Infrastructure.Persistence;
using MyApi.Infrastructure.Services;
using MyApi.Application.Common.Configs;

namespace MyApi.Infrastructure
{
  public static class AppDependencyServices
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
      var dbPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");

      var connectionString = string.Format(
          configuration.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
          dbHost, dbPort, dbName, dbUser, dbPassword);

      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(connectionString,
              sqlServerOptionsAction: sqlOptions =>
              {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
              }));

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
          policy.WithOrigins("http://localhost:3000") //replace with front end host
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); //* allow cookies on header
        });
      });

      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
      services.AddControllers().AddJsonOptions(options =>
      {
        // Disable property name conversion camelcase and keep original property names from C# models
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // Allow case-insensitive property name matching when deserializing JSON
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
      });

      services.AddMediatR(typeof(Program).Assembly);
      services.AddScoped<IAssetPathService, AssetPathService>();

      return services;
    }

    private static IServiceCollection BindConfig(
        this IServiceCollection services,
        IConfiguration configuration)
    {
      services.Configure<StorageConfig>(configuration.GetSection("StorageConfig"));
      services.Configure<CredentialConfig>(config =>
      {
        config.Google = Environment.GetEnvironmentVariable("GOOGLE") ?? "";
        config.GoogleApiKey = Environment.GetEnvironmentVariable("GOOGLE_KEY") ?? "";
      });
      return services;
    }

    private static IServiceCollection BindMapper(
        this IServiceCollection services)
    {
      services.AddAutoMapper(typeof(Program).Assembly);
      return services;
    }
  }

}