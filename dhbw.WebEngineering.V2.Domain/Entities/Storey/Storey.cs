using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Storey;

public record Storey
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid Building_id { get; set; }
    public required DateTime? Deleted_at { get; set; }

    private Storey() { }

    // Constructor for JSON deserialization
    [JsonConstructor]
    public Storey(Guid id, string name, Guid building_id, DateTime? deleted_at)
    {
        Id = id;
        Name = name;
        Building_id = building_id;
        Deleted_at = deleted_at;
    }

    public static Storey Create(string name, Guid building_id)
    {
        return new Storey
        {
            Id = Guid.NewGuid(),
            Name = name,
            Building_id = building_id,
            Deleted_at = null,
        };
    }
}
