using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Room;

public record ReadRoomDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid Storey_id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required DateTime? Deleted_at { get; set; }
}
