namespace dhbw.WebEngineering.V2.Domain.Entities.Room;

public record CreateRoomDto
{
    public required string name { get; set; }
    public required Guid storey_id { get; set; }
}
