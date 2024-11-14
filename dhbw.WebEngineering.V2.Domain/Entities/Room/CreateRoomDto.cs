namespace dhbw.WebEngineering.V2.Domain.Entities.Room;

public record CreateRoomDto
{
    public required string Name { get; set; }
    public required Guid Storey_id { get; set; }
}
