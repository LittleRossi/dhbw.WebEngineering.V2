using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Storey;

public record Storey
{
    public required Guid id { get; set; }
    public required string name { get; set; }
    public required Guid building_id { get; set; }
    public required DateTime? deleted_at { get; set; }

    private Storey() { }

    // Constructor for JSON deserialization
    [JsonConstructor]
    public Storey(Guid id, string name, Guid building_id, DateTime? deleted_at)
    {
        this.id = id;
        this.name = name;
        this.building_id = building_id;
        this.deleted_at = deleted_at;
    }

    public static Storey Create(string name, Guid building_id)
    {
        return new Storey
        {
            id = Guid.NewGuid(),
            name = name,
            building_id = building_id,
            deleted_at = null,
        };
    }
}
