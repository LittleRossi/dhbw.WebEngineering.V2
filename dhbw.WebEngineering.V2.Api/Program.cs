using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Api.Endpoints;
using dhbw.WebEngineering.V2.Api.Extensions;
using dhbw.WebEngineering.V2.Domain.Storey;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(); // Add environment variables to override appsettings.json

// Add services to the container
builder.Services.AddPostgresDbContext(builder.Configuration); // Configure Postgres database connection
builder.Services.AddApplicationServices(); // Register application services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtAuthorization(); // Configure Swagger with JWT authorization
builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("assets", HealthStatus.Unhealthy); // add Health-Check
await builder.Services.AddJwtAuthenticationAsync(builder.Configuration, builder.Environment); // Configure JWT Authentication
builder.Services.AddCustomJsonConverters(); // Add custom JSON converters to services

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddBuildingEndpoints();
app.AddStatusEndpoints();
app.AddRoomEndpoints();
app.AddHealthEndpoints();
app.AddStoreyEndpoints();

app.Run();
