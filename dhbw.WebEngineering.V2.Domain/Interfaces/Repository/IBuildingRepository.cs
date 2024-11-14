using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Building;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Repository;

public interface IBuildingRepository
{
    public Task<Maybe<List<Building>>> GetAllAsync(bool includeDeleted = false);
    public Task<Maybe<Building>> GetByIdAsync(Guid id);
    public Task<Maybe<Building>> CreateAsync(Building entity);
    public Task<Maybe<Building>> UpdateAsync(Building entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
