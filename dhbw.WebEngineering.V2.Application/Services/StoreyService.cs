using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Storey;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
using dhbw.WebEngineering.V2.Domain.Interfaces.Service;

namespace dhbw.WebEngineering.V2.Application.Services;

public class StoreyService : IStoreyService
{
    private readonly IStoreyRepository _storeyRepository;

    public StoreyService(IStoreyRepository storeyRepository)
    {
        _storeyRepository = storeyRepository;
    }

    public async Task<Result<List<Storey>>> GetAllAsync(
        Guid building_id,
        bool includeDeleted = false
    )
    {
        return await _storeyRepository.GetAllAsync().ToResult("No Storeys found");
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