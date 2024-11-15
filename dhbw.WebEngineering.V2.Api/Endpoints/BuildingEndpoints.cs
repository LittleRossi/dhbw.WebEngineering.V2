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
            .WithName("Get all Buildings")
            .WithOpenApi()
            .WithTags("Buildings");

        app.MapGet(
                "/api/v3/assets/buildings/{id}",
                async ([FromServices] IBuildingService service, Guid id) =>
                {
                    var fetchedBuilding = await service.GetByIdAsync(id).Map(BuildingMapper.ToDto);

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

                    var createdEntity = await BuildingMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(BuildingMapper.ToDto);

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

                    var updatedEntity = await BuildingMapper
                        .ToEntity(entity)
                        .Bind(mappedEntity => service.UpdateAsync(mappedEntity, id))
                        .Map(BuildingMapper.ToDto);

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
