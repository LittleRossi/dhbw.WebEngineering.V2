using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Building;

public interface IBuildingRepository
{
    public Task<Maybe<List<Building>>> GetAllAsync(bool includeDeleted = false);
    public Task<Maybe<Building>> GetByIdAsync(Guid id);
    public Task<Maybe<Building>> CreateAsync(Building entity);
    public Task<Maybe<Building>> UpdateAsync(Building entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
