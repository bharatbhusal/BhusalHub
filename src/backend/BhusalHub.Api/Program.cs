using BhusalHub.Api.Middleware;
using BhusalHub.Core.Interfaces;
using BhusalHub.Core.Services;
using BhusalHub.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var dbPath = builder.Configuration.GetValue<string>("Database:Path") ?? "bhusalhub.db";
builder.Services.AddSingleton(new DatabaseInitializer(dbPath));
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IHealthService, HealthService>();

var app = builder.Build();

app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

public partial class Program { }
