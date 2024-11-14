using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Domain.Entities.Room;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace dhbw.WebEngineering.V2.Adapters.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _appDbContext;

    public RoomRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Maybe<List<Room>>> GetAllAsync(bool includeDeleted = false)
    {
        return includeDeleted
            ? await _appDbContext.rooms.IgnoreQueryFilters().ToListAsync()
            : await _appDbContext.rooms.ToListAsync();
    }

    public async Task<Maybe<Room>> GetByIdAsync(Guid id)
    {
        return await _appDbContext.rooms.FindAsync(id);
    }

    public async Task<Maybe<Room>> CreateAsync(Room entity)
    {
        var result = await _appDbContext.rooms.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return Maybe<Room>.From(result.Entity);
    }

    public async Task<Maybe<Room>> UpdateAsync(Room entity, Guid id)
    {
        var existingRoom = await _appDbContext.rooms.FindAsync(id);

        if (existingRoom == null)
            return null;

        existingRoom.Name = entity.Name;
        existingRoom.Storey_id = entity.Storey_id;
        existingRoom.Deleted_at = entity.Deleted_at;

        await _appDbContext.SaveChangesAsync();

        return existingRoom;
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        var room = await _appDbContext.rooms.FindAsync(id);

        if (room == null)
        {
            return Result.Failure($"No existing Room with ID: {id}");
        }

        room.Deleted_at = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}
