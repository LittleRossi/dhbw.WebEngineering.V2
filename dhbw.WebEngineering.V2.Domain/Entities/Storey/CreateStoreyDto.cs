namespace dhbw.WebEngineering.V2.Domain.Entities.Storey;

public record CreateStoreyDto
{
    public required string Name { get; set; }
    public required Guid Building_id { get; set; }
}
