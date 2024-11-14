using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Building;

public record ReadBuildingDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Streetname { get; set; }
    public required string Housenumber { get; set; }
    public required string Country_code { get; set; }
    public required string Postalcode { get; set; }
    public required string City { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? Deleted_at { get; set; }
}
