using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Room;

public class RoomMapper
{
    public static Result<Room> ToEntity(CreateRoomDto createRoomDto)
    {
        return Room.Create(createRoomDto.name, createRoomDto.storey_id);
    }

    public static ReadRoomDto ToDto(Room room)
    {
        return new ReadRoomDto
        {
            Id = room.id,
            Name = room.name,
            Storey_id = room.storey_id,
            Deleted_at = room.deleted_at,
        };
    }

    public static List<ReadRoomDto> ToDto(List<Room> rooms)
    {
        var result = new List<ReadRoomDto>();
        rooms.ForEach(room => result.Add(ToDto(room)));

        return result;
    }

    public static RoomResponse ToRoomResponse(List<ReadRoomDto> rooms)
    {
        return new RoomResponse(rooms);
    }
}
