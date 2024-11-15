using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Building;

namespace dhbw.WebEngineering.V2.Application.Services;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;

    public BuildingService(IBuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository;
    }

    public async Task<Result<List<Building>>> GetAllAsync(bool includeDeleted = false)
    {
        return await _buildingRepository.GetAllAsync(includeDeleted).ToResult("No Buildings found");
    }

    public async Task<Result<Building>> GetByIdAsync(Guid id)
    {
        return await _buildingRepository
            .GetByIdAsync(id)
            .ToResult($"No existing Building with the Id: {id}");
    }

    public async Task<Result<Building>> CreateNewAsync(Building entity)
    {
        return await _buildingRepository
            .CreateAsync(entity)
            .ToResult("An Error happened while trying to Create a Building");
    }

    public async Task<Result<Building>> UpdateAsync(Building entity, Guid id)
    {
        return await _buildingRepository
            .UpdateAsync(entity, id)
            .ToResult("An Error happened while trying to Update a Building");
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        return await _buildingRepository.DeleteAsync(id, permanent);
    }
}
