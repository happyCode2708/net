using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MyApi.Application.Common.Configs;
using MyApi.Application.Common.Interfaces;
using MyApi.Application.Services;
using MyApi.Infrastructure.Persistence;
using AutoMapper;
using MyApi.Application.Common.Utils;

var builder = WebApplication.CreateBuilder(args);

//* Register IHttpClientFactory
builder.Services.AddHttpClient(); // This registers the IHttpCdlientFactory service

//* add db connection
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//* add services to scope
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IImageServices, ImageServices>();
builder.Services.AddSingleton<IGenerativeServices, GenerativeServices>();
builder.Services.AddSingleton<IGeminiServices, GeminiServices>();
builder.Services.AddSingleton<IPromptBuilderService, PromptBuilderService>();

//* add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your Next.js app URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // if you need to send cookies or auth headers
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();
// Add this line to register controllers


// Register MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

//* bind config
builder.Services.Configure<StorageConfig>(builder.Configuration.GetSection("StorageConfig"));
builder.Services.Configure<CredentialConfig>(builder.Configuration.GetSection("Credentials"));

//* add auto mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//* static config
var assetPath = builder.Configuration["StorageConfig:AssetPath"];
var assetPathRequest = builder.Configuration["StorageConfig:AssetPathRequest"];

//* Register AutoMapper

var app = builder.Build();


//* use cors policy
app.UseCors("AllowNextJsApp");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), assetPath)),
    RequestPath = assetPathRequest,
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapControllers(); // This enables routing for controllers.

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
