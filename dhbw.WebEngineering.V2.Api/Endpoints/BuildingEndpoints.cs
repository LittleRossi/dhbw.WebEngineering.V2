using System.Security.Claims;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Entities.Building;
using dhbw.WebEngineering.V2.Domain.Interfaces.Service;
using dhbw.WebEngineering.V2.Domain.Mapper;
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
                        .Bind(BuildingMapper.ToDto)
                        .ToOkHttpResult()
            )
            .WithName("Get all Buildings")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapGet(
                "/api/v3/assets/buildings/{id}",
                async ([FromServices] IBuildingService service, Guid id) =>
                {
                    var fetchedBuilding = await service.GetByIdAsync(id);

                    if (fetchedBuilding.IsFailure)
                        return Results.NotFound(fetchedBuilding.Error);

                    return fetchedBuilding.ToOkHttpResult();
                }
            )
            .WithName("Get Building by Id")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapPost(
                "/api/v3/assets/buildings",
                async (
                    [FromServices] IBuildingService service,
                    Building entity,
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
                    var createdEntity = await service.CreateNewAsync(entity);

                    if (createdEntity.IsFailure)
                        return createdEntity.ToCreatedHttpResult();

                    var locationUri = $"/api/v3/assets/buildings/{createdEntity.Value.Id}";
                    return Results.Created(locationUri, createdEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("Create new Building")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapPut(
                "/api/v3/assets/buildings/{id}",
                async (
                    [FromServices] IBuildingService service,
                    Guid id,
                    Building entity,
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
                    var updatedEntity = await service.UpdateAsync(entity, id);

                    if (updatedEntity.IsFailure)
                        return Results.NotFound(updatedEntity.Error);

                    return Results.Ok(updatedEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("Update Building")
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
                    var deleteSuccessfull = await service.DeleteAsync(id, permanent);

                    if (deleteSuccessfull.IsFailure)
                        return Results.BadRequest(deleteSuccessfull.Error);

                    return Results.NoContent();
                }
            )
            .RequireAuthorization()
            .WithName("Delete Building")
            .WithOpenApi()
            .WithTags("Buildings");
    }
}
