using System.Security.Claims;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
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
                    await service
                        .GetAllAsync(building_id, include_deleted)
                        .Map(StoreyMapper.ToDto)
                        .Map(StoreyMapper.ToStoreyResponse)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status204NoContent)
            )
            .WithName("GetStoreys")
            .WithOpenApi()
            .WithTags("Storeys");

        app.MapGet(
                "/api/v3/assets/storeys/{id}",
                async ([FromServices] IStoreyService service, Guid id) =>
                    await service
                        .GetByIdAsync(id)
                        .Map(StoreyMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound)
            )
            .WithName("GetStoreyById")
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
            .WithName("DeleteStorey")
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

                    return await StoreyMapper
                        .ToEntity(entity)
                        .Bind(service.CreateNewAsync)
                        .Map(StoreyMapper.ToDto)
                        .ToCreatedHttpResult(s => new Uri(
                            $"/api/v3/assets/storeys/{s.Id.ToString()}",
                            UriKind.Relative
                        ));
                }
            )
            .RequireAuthorization()
            .WithName("CreateNewStorey")
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
                    return await service
                        .UpdateAsync(entity, id)
                        .Map(StoreyMapper.ToDto)
                        .ToOkHttpResult(failureStatusCode: StatusCodes.Status404NotFound);
                }
            )
            .RequireAuthorization()
            .WithName("UpdateStorey")
            .WithOpenApi()
            .WithTags("Storeys");
    }
}
