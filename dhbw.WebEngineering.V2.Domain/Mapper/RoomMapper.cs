using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Room;

namespace dhbw.WebEngineering.V2.Domain.Mapper;

public class RoomMapper
{
    public static Result<Room> ToEntity(CreateRoomDto createRoomDto)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(createRoomDto.name))
        {
            return Result.Failure<Room>("Name cannot be empty.");
        }
        if (createRoomDto.storey_id == Guid.Empty)
        {
            return Result.Failure<Room>("Storey_id cannot be an empty GUID.");
        }
        #endregion

        return Room.Create(createRoomDto.name, createRoomDto.storey_id);
    }

    public static ReadRoomDto ToDto(Room room)
    {
        return new ReadRoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Storey_id = room.Storey_id,
            Deleted_at = room.Deleted_at,
        };
    }

    public static List<ReadRoomDto> ToDto(List<Room> rooms)
    {
        var result = new List<ReadRoomDto>();
        rooms.ForEach(room => result.Add(ToDto(room)));

        return result;
    }
}
