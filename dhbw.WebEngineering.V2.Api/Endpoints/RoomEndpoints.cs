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
                    await service
                        .GetAllAsync(storey_id, include_deleted)
                        .Map(RoomMapper.ToDto)
                        .Map(RoomMapper.ToRoomResponse)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status204NoContent)
            )
            .WithName("GetAllRooms")
            .WithOpenApi()
            .WithTags("Rooms");

        app.MapGet(
                "/api/v3/assets/rooms/{id}",
                async ([FromServices] IRoomService service, Guid id) =>
                    await service
                        .GetByIdAsync(id)
                        .Map(RoomMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound)
            )
            .WithName("GetRoomById")
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

                    return await RoomMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(RoomMapper.ToDto)
                        .ToCreatedHttpResult(r => new Uri(
                            $"/api/v3/assets/rooms/{r.Id.ToString()}",
                            UriKind.Relative
                        ));
                }
            )
            .RequireAuthorization()
            .WithName("CreateNewRoom")
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
                    return await service
                        .DeleteAsync(id, permanent)
                        .ToNoContentHttpResult(failureStatusCode: StatusCodes.Status400BadRequest);
                }
            )
            .RequireAuthorization()
            .WithName("DeleteRoom")
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

                    return await RoomMapper
                        .ToEntity(entity)
                        .Bind(mappedEntity => service.UpdateAsync(mappedEntity, id))
                        .Map(RoomMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound);
                }
            )
            .RequireAuthorization()
            .WithName("UpdateRoom")
            .WithOpenApi()
            .WithTags("Rooms");
    }
}
