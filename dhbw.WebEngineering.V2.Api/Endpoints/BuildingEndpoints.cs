using System.Security.Claims;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Building;
using Microsoft.AspNetCore.Mvc;

namespace dhbw.WebEngineering.V2.Api.Endpoints;

public static class BuildingEndpoints
{
    public static void AddBuildingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/v3/assets/buildings",
                async (
                    [FromServices] IBuildingService service,
                    [FromQuery] bool include_deleted = false
                ) =>
                    await service
                        .GetAllAsync(include_deleted)
                        .Map(BuildingMapper.ToDto)
                        .Map(BuildingMapper.ToBuildingsResponse)
                        .ToOkHttpResult()
            )
            .WithName("GetAllBuildings")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapGet(
                "/api/v3/assets/buildings/{id}",
                async ([FromServices] IBuildingService service, Guid id) =>
                    await service
                        .GetByIdAsync(id)
                        .Map(BuildingMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound)
            )
            .WithName("GetBuildingById")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapPost(
                "/api/v3/assets/buildings",
                async (
                    [FromServices] IBuildingService service,
                    CreateBuildingDto entity,
                    ClaimsPrincipal user,
                    ILogger<BuildingService> logger
                ) =>
                {
                    var id = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation(
                        "User {id} creates {type} with value: {entity}",
                        id,
                        entity.GetType().Name,
                        entity
                    );

                    return await BuildingMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(BuildingMapper.ToDto)
                        .ToCreatedHttpResult(b => new Uri(
                            $"/api/v3/assets/buildings/{b.Id.ToString()}",
                            UriKind.Relative
                        ));
                }
            )
            .RequireAuthorization()
            .WithName("CreateNewBuilding")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapPut(
                "/api/v3/assets/buildings/{id}",
                async (
                    [FromServices] IBuildingService service,
                    Guid id,
                    CreateBuildingDto entity,
                    ClaimsPrincipal user,
                    ILogger<BuildingService> logger
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation(
                        "User {uId} updates {type} with value: {entity}",
                        uId,
                        entity.GetType().Name,
                        entity
                    );

                    return await BuildingMapper
                        .ToEntity(entity)
                        .Bind(mappedEntity => service.UpdateAsync(mappedEntity, id))
                        .Map(BuildingMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound);
                }
            )
            .RequireAuthorization()
            .WithName("UpdateBuilding")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapDelete(
                "/api/v3/assets/buildings/{id}",
                async (
                    [FromServices] IBuildingService service,
                    Guid id,
                    ClaimsPrincipal user,
                    ILogger<BuildingService> logger,
                    [FromQuery] bool permanent = false
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation("User {uId} deletes with id: {uId}", uId, uId);

                    return await service
                        .DeleteAsync(id, permanent)
                        .ToNoContentHttpResult(failureStatusCode: StatusCodes.Status400BadRequest);
                }
            )
            .RequireAuthorization()
            .WithName("DeleteBuilding")
            .WithOpenApi()
            .WithTags("Buildings");
    }
}
