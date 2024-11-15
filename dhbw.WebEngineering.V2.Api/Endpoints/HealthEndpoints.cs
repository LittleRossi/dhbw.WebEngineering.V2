using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace dhbw.WebEngineering.V2.Api.Endpoints;

public static class HealthEndpoints
{
    public static void AddHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/v3/assets/health",
                async (HealthCheckService healthCheckService) =>
                {
                    var report = await healthCheckService.CheckHealthAsync();

                    var assetsConnected =
                        report.Entries.ContainsKey("assets")
                        && report.Entries["assets"].Data is Dictionary<string, object> data
                        && data.ContainsKey("connected")
                        && (bool)data["connected"];

                    var healthStatus = new
                    {
                        live = true,
                        ready = report.Status == HealthStatus.Healthy,
                        databases = new { assets = new { connected = assetsConnected } },
                    };

                    return Results.Ok(healthStatus);
                }
            )
            .WithName("HealthChecks")
            .WithOpenApi()
            .WithTags("Status");

        app.MapGet(
                "/api/v3/assets/health/live",
                () =>
                {
                    var healthStatus = new { live = true };

                    return Results.Ok(healthStatus);
                }
            )
            .WithName("LivenessCheck")
            .WithOpenApi()
            .WithTags("Status");

        app.MapGet(
                "/api/v3/assets/health/ready",
                async (HealthCheckService healthCheckService) =>
                {
                    var report = await healthCheckService.CheckHealthAsync();

                    var healthStatus = new { ready = report.Status == HealthStatus.Healthy };

                    return Results.Ok(healthStatus);
                }
            )
            .WithName("ReadyCheck")
            .WithOpenApi()
            .WithTags("Status");
    }
}
