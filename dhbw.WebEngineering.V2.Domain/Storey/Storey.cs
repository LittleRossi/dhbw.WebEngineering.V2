using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Storey;

public record Storey
{
    public required Guid id { get; set; }
    public required string name { get; set; }
    public required Guid building_id { get; set; }
    public required DateTime? deleted_at { get; set; }

    private Storey() { }

    public static Result<Storey> Create(string name, Guid building_id)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Storey>("Name cannot be empty.");
        }

        if (building_id == Guid.Empty)
        {
            return Result.Failure<Storey>("Building_id cannot be an empty GUID.");
        }
        #endregion

        return new Storey
        {
            id = Guid.NewGuid(),
            name = name,
            building_id = building_id,
            deleted_at = null,
        };
    }
}
