using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Room;

public record Room
{
    public required Guid id { get; set; }
    public required string name { get; set; }
    public required Guid storey_id { get; set; }
    public required DateTime? deleted_at { get; set; }

    private Room() { }

    [JsonConstructor]
    public Room(Guid id, string name, Guid storey_id, DateTime? deleted_at)
    {
        this.id = id;
        this.name = name;
        this.storey_id = storey_id;
        this.deleted_at = deleted_at;
    }

    public static Room Create(string name, Guid storey_id)
    {
        return new Room
        {
            id = Guid.NewGuid(),
            name = name,
            storey_id = storey_id,
            deleted_at = null,
        };
    }
}
