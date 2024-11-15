using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Building;

public interface IBuildingService
{
    public Task<Result<List<Building>>> GetAllAsync(bool includeDeleted = false);
    public Task<Result<Building>> GetByIdAsync(Guid id);
    public Task<Result<Building>> CreateNewAsync(Building entity);
    public Task<Result<Building>> UpdateAsync(Building entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
