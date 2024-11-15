using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Room;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
using dhbw.WebEngineering.V2.Domain.Interfaces.Service;

namespace dhbw.WebEngineering.V2.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<Room>>> GetAllAsync(Guid? storey_id, bool includeDeleted = false)
    {
        var allRooms = await _roomRepository.GetAllAsync().ToResult("No Rooms found");

        if (storey_id == null)
            return allRooms;

        return allRooms.Value.Where(s => s.storey_id == storey_id).ToList();
    }

    public async Task<Result<Room>> GetByIdAsync(Guid id)
    {
        return await _roomRepository.GetByIdAsync(id).ToResult($"No existing Room with ID: {id}");
    }

    public async Task<Result<Room>> CreateNewAsync(Room entity)
    {
        return await _roomRepository
            .CreateAsync(entity)
            .ToResult("An Error happened while trying to Create a Room");
    }

    public async Task<Result<Room>> UpdateAsync(Room entity, Guid id)
    {
        return await _roomRepository
            .UpdateAsync(entity, id)
            .ToResult("An Error happened while trying to Update a Room");
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        return await _roomRepository.DeleteAsync(id, permanent);
    }
}
