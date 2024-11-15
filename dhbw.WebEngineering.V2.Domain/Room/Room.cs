using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Room;

public record Room
{
    public required Guid id { get; set; }
    public required string name { get; set; }
    public required Guid storey_id { get; set; }
    public required DateTime? deleted_at { get; set; }

    private Room() { }

    public static Result<Room> Create(string name, Guid storey_id)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Room>("Name cannot be empty.");
        }
        if (storey_id == Guid.Empty)
        {
            return Result.Failure<Room>("Storey_id cannot be an empty GUID.");
        }
        #endregion

        return new Room
        {
            id = Guid.NewGuid(),
            name = name,
            storey_id = storey_id,
            deleted_at = null,
        };
    }
}
