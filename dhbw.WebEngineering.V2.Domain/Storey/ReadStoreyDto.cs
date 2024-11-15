using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Storey;

public record ReadStoreyDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid Building_id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required DateTime? Deleted_at { get; set; }
}
