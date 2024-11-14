namespace dhbw.WebEngineering.V2.Domain.Entities.Room;

public record RoomResponse
{
    public List<ReadRoomDto> Rooms { get; set; } = new List<ReadRoomDto>();

    public RoomResponse(List<ReadRoomDto> rooms)
    {
        Rooms = rooms;
    }
}
