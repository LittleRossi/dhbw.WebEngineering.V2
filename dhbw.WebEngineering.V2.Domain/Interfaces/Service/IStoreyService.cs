using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Storey;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Service;

public interface IStoreyService
{
    public Task<Result<List<Storey>>> GetAllAsync(Guid building_id, bool includeDeleted = false);
    public Task<Result<Storey>> GetByIdAsync(Guid id);
    public Task<Result<Storey>> CreateNewAsync(Storey entity);
    public Task<Result<Storey>> UpdateAsync(Storey entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
