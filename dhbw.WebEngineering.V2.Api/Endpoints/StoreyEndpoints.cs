using System.Security.Claims;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Storey;
using Microsoft.AspNetCore.Mvc;

namespace dhbw.WebEngineering.V2.Api.Endpoints;

public static class StoreyEndpoints
{
    public static void AddStoreyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/v3/assets/storeys",
                async (
                    [FromServices] IStoreyService service,
                    [FromQuery] bool include_deleted = false,
                    [FromQuery] Guid? building_id = default
                ) =>
                {
                    var storeys = await service
                        .GetAllAsync(building_id, include_deleted)
                        .Map(StoreyMapper.ToDto)
                        .Map(StoreyMapper.ToStoreyResponse);

                    if (storeys.IsFailure)
                        return Results.NoContent();

                    return Results.Ok(storeys.Value);
                }
            )
            .WithName("Get Storeys")
            .WithOpenApi()
            .WithTags("Storeys");

        app.MapGet(
                "/api/v3/assets/storeys/{id}",
                async ([FromServices] IStoreyService service, Guid id) =>
                {
                    var result = await service.GetByIdAsync(id).Map(StoreyMapper.ToDto);

                    if (result.IsFailure)
                        return Results.NotFound(result.Error);

                    return Results.Ok(result.Value);
                }
            )
            .WithName("Get Storey by Id")
            .WithOpenApi()
            .WithTags("Storeys");

        app.MapDelete(
                "/api/v3/assets/storeys/{id}",
                async (
                    [FromServices] IStoreyService service,
                    Guid id,
                    ClaimsPrincipal user,
                    ILogger<StoreyService> logger,
                    [FromQuery] bool permanent = false
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation("User {uId} deletes Storey id: {id}", uId, id);

                    var deletion = await service.DeleteAsync(id, permanent);

                    if (deletion.IsFailure)
                        return Results.BadRequest(deletion.Error);

                    return Results.NotFound();
                }
            )
            .RequireAuthorization()
            .WithName("Delete Storey")
            .WithOpenApi()
            .WithTags("Storeys");

        app.MapPost(
                "/api/v3/assets/storeys",
                async (
                    [FromServices] IStoreyService service,
                    CreateStoreyDto entity,
                    ClaimsPrincipal user,
                    ILogger<RoomService> logger
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation(
                        "User {uId} creates {type} with value: {entity}",
                        uId,
                        entity.GetType().Name,
                        entity
                    );

                    var createdEntity = await StoreyMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(StoreyMapper.ToDto);

                    if (createdEntity.IsFailure)
                        return Results.BadRequest(createdEntity.Error);

                    var locationUri = $"/api/v3/assets/storeys/{createdEntity.Value.Id}";

                    return Results.Created(locationUri, createdEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("Create new Storey")
            .WithOpenApi()
            .WithTags("Storeys");

        app.MapPut(
                "/api/v3/assets/storeys/{id}",
                async (
                    [FromServices] IStoreyService service,
                    Guid id,
                    Storey entity,
                    ClaimsPrincipal user,
                    ILogger<RoomService> logger
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation(
                        "User {uId} updates {type} with value: {entity}",
                        uId,
                        entity.GetType().Name,
                        entity
                    );
                    var updatedEntity = await service
                        .UpdateAsync(entity, id)
                        .Map(StoreyMapper.ToDto);

                    if (updatedEntity.IsFailure)
                        return Results.NotFound(updatedEntity.Error);

                    return Results.Ok(updatedEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("UpdateStorey")
            .WithOpenApi()
            .WithTags("Storeys");
    }
}
