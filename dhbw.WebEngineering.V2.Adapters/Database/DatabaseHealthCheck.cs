using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace dhbw.WebEngineering.V2.Adapters.Database;

public sealed class DatabaseHealthCheck(AppDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        try
        {
            bool canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

            var healthData = new Dictionary<string, object> { { "connected", canConnect } };

            return canConnect
                ? HealthCheckResult.Healthy(null, healthData)
                : HealthCheckResult.Unhealthy(null, null, healthData);
        }
        catch (Exception e)
        {
            var healthData = new Dictionary<string, object> { { "connected", false } };
            return HealthCheckResult.Unhealthy(description: null, exception: e, healthData);
        }
    }
}
