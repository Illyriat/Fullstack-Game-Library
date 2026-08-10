using Game_Library_Service.Common.Extensions.Startup;
using Game_Library_Service.Common.HealthCheck;
using Game_Library_Service.Common.OpenApi;
using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Filters;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
var corsPolicyName = builder.Services.ConfigureCors(builder.Configuration);

// Add services to the container
builder.Services.AddControllers(options => options.Filters.Add(new ExceptionFilter()))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();

// Configure Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Mediator
builder.Services.ConfigureMediatorAndHandlers();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// Configure Scalar UI
builder.Services.AddScalarOpenApi();

var app = builder.Build();

app.MapHealthChecks("/status", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteResponse
});

// Configure the HTTP request pipeline
app.MapScalarForDev();

app.UseCors(corsPolicyName);
app.UseHttpsRedirection();

app.MapControllers();

// Apply pending migrations at startup
await using var scope = app.Services.CreateAsyncScope();
await using var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await db.Database.MigrateAsync();

app.Run();
