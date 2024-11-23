using Microsoft.Extensions.FileProviders;
using Extensions;
using MyApi.Infrastructure;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration.AddEnvironmentVariables();

//* add infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

//* static config
var assetPath = builder.Configuration["StorageConfig:AssetPath"];
var assetPathRequest = builder.Configuration["StorageConfig:AssetPathRequest"];

//* Register AutoMapper

var app = builder.Build();

//* apply migrations
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

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

app.MapControllers(); // This enables routing for controllers.

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
