using dhbw.WebEngineering.V2.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace dhbw.WebEngineering.V2.Api.Endpoints;

public static class StatusEndpoints
{
    public static void AddStatusEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/v3/assets/status",
                ([FromServices] IStatusService service) => service.GetStatusInformation()
            )
            .WithName("Get Status")
            .WithOpenApi()
            .WithTags("Status");
    }
}