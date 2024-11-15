using System.Security.Claims;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Room;
using Microsoft.AspNetCore.Mvc;

namespace dhbw.WebEngineering.V2.Api.Endpoints;

public static class RoomEndpoints
{
    public static void AddRoomEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/v3/assets/rooms",
                async (
                    [FromServices] IRoomService service,
                    [FromQuery] bool include_deleted = false,
                    [FromQuery] Guid? storey_id = default
                ) =>
                {
                    var fetchedLists = await service
                        .GetAllAsync(storey_id, include_deleted)
                        .Map(RoomMapper.ToDto)
                        .Map(RoomMapper.ToRoomResponse);

                    if (fetchedLists.IsFailure)
                        return Results.NoContent();

                    return Results.Ok(fetchedLists.Value);
                }
            )
            .WithName("Get all Rooms")
            .WithOpenApi()
            .WithTags("Rooms");

        app.MapGet(
                "/api/v3/assets/rooms/{id}",
                async ([FromServices] IRoomService service, Guid id) =>
                {
                    var fetchedBuilding = await service.GetByIdAsync(id).Map(RoomMapper.ToDto);

                    if (fetchedBuilding.IsFailure)
                        return Results.NotFound(fetchedBuilding.Error);

                    return fetchedBuilding.ToOkHttpResult();
                }
            )
            .WithName("Get Room by Id")
            .WithOpenApi()
            .WithTags("Rooms");

        app.MapPost(
                "/api/v3/assets/rooms",
                async (
                    [FromServices] IRoomService service,
                    CreateRoomDto entity,
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

                    var createdEntity = await RoomMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(RoomMapper.ToDto);

                    if (createdEntity.IsFailure)
                        return createdEntity.ToCreatedHttpResult();

                    var locationUri = $"/api/v3/assets/rooms/{createdEntity.Value.Id}";
                    return Results.Created(locationUri, createdEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("Create new Room")
            .WithOpenApi()
            .WithTags("Rooms");

        app.MapDelete(
                "/api/v3/assets/rooms/{id}",
                async (
                    [FromServices] IRoomService service,
                    Guid id,
                    ClaimsPrincipal user,
                    ILogger<RoomService> logger,
                    [FromQuery] bool permanent = false
                ) =>
                {
                    var uId = user.Claims.First(c => c.Type == "sid").Value;
                    logger.LogInformation("User {uId} deletes Room id: {id}", uId, id);
                    var deleteSuccessfull = await service.DeleteAsync(id, permanent);

                    if (deleteSuccessfull.IsFailure)
                        return Results.BadRequest(deleteSuccessfull.Error);

                    return Results.NoContent();
                }
            )
            .RequireAuthorization()
            .WithName("Delete Room")
            .WithOpenApi()
            .WithTags("Rooms");

        app.MapPut(
                "/api/v3/assets/rooms/{id}",
                async (
                    [FromServices] IRoomService service,
                    Guid id,
                    CreateRoomDto entity,
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

                    var updatedEntity = await RoomMapper
                        .ToEntity(entity)
                        .Bind(mappedEntity => service.UpdateAsync(mappedEntity, id))
                        .Map(RoomMapper.ToDto);

                    if (updatedEntity.IsFailure)
                        return Results.NotFound(updatedEntity.Error);

                    return Results.Ok(updatedEntity.Value);
                }
            )
            .RequireAuthorization()
            .WithName("Update Room")
            .WithOpenApi()
            .WithTags("Rooms");
    }
}
