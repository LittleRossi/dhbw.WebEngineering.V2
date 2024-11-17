using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Storey;

namespace dhbw.WebEngineering.V2.Application.Services;

public class StoreyService : IStoreyService
{
    private readonly IStoreyRepository _storeyRepository;

    public StoreyService(IStoreyRepository storeyRepository)
    {
        _storeyRepository = storeyRepository;
    }

    public async Task<Result<List<Storey>>> GetAllAsync(
        Guid? building_id,
        bool includeDeleted = false
    )
    {
        var storeys = await _storeyRepository
            .GetAllAsync(includeDeleted)
            .ToResult("No Storeys found");

        if (building_id == null)
            return storeys;

        return storeys.Value.Where(s => s.building_id == building_id).ToList();
    }

    public async Task<Result<Storey>> GetByIdAsync(Guid id)
    {
        return await _storeyRepository
            .GetByIdAsync(id)
            .ToResult($"No existing Storey with ID: {id}");
    }

    public async Task<Result<Storey>> CreateNewAsync(Storey entity)
    {
        return await _storeyRepository
            .CreateAsync(entity)
            .ToResult("An Error happened while trying to Create a Storey");
    }

    public async Task<Result<Storey>> UpdateAsync(Storey entity, Guid id)
    {
        return await _storeyRepository
            .UpdateAsync(entity, id)
            .ToResult("An Error happened while trying to Update a Storey");
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        return await _storeyRepository.DeleteAsync(id, permanent);
    }
}
