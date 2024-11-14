using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Storey;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Repository;

public interface IStoreyRepository
{
    public Task<Maybe<List<Storey>>> GetAllAsync();
    public Task<Maybe<Storey>> GetByIdAsync(Guid id);
    public Task<Maybe<Storey>> CreateAsync(Storey entity);
    public Task<Maybe<Storey>> UpdateAsync(Storey entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
