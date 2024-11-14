using dhbw.WebEngineering.V2.Adapters.Repositories;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
using dhbw.WebEngineering.V2.Domain.Interfaces.Service;

namespace dhbw.WebEngineering.V2.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBuildingRepository, BuildingRepository>();
        services.AddScoped<IBuildingService, BuildingService>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IStoreyRepository, StoreyRepository>();
        services.AddScoped<IStoreyService, StoreyService>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IStatusService, StatusService>();
    }
}