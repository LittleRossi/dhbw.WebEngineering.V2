using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Room;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Service;

public interface IRoomService
{
    public Task<Result<List<Room>>> GetAllAsync(Guid? storey_id, bool includeDeleted = false);
    public Task<Result<Room>> GetByIdAsync(Guid id);
    public Task<Result<Room>> CreateNewAsync(Room entity);
    public Task<Result<Room>> UpdateAsync(Room entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}
