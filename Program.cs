using MyApi.Infrastructure;
using DotNetEnv;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var envFileName = $".env.{environment.ToLower()}";
DotNetEnv.Env.Load(envFileName);
// DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

//* add infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

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

app.UseStaticFilesByAssetPath(builder.Configuration);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
