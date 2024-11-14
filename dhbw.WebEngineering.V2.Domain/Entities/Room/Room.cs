using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Room;

public record Room
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid Storey_id { get; set; }
    public required DateTime? Deleted_at { get; set; }

    private Room() { }

    [JsonConstructor]
    public Room(Guid id, string name, Guid storey_id, DateTime? deleted_at)
    {
        Id = id;
        Name = name;
        Storey_id = storey_id;
        Deleted_at = deleted_at;
    }

    public static Room Create(string name, Guid storey_id)
    {
        return new Room
        {
            Id = Guid.NewGuid(),
            Name = name,
            Storey_id = storey_id,
            Deleted_at = null,
        };
    }
}
