using Microsoft.Extensions.FileProviders;
using Extensions;
using MyApi.Infrastructure;
using DotNetEnv;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration.AddEnvironmentVariables();

//* add infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

//* static config
var assetPath = Environment.GetEnvironmentVariable("ASSET_PATH") ?? builder.Configuration["StorageConfig:AssetPath"];
var assetPathRequest = builder.Configuration["StorageConfig:AssetPathRequest"];

//* Register AutoMapper

var app = builder.Build();

app.UseDbReSeed();

//* apply migrations
if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseDeveloperExceptionPage();
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

app.MapControllers();

app.Run();
