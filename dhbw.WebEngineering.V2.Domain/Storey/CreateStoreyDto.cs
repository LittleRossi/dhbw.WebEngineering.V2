namespace dhbw.WebEngineering.V2.Domain.Storey;

public record CreateStoreyDto
{
    public required string Name { get; set; }
    public required Guid Building_id { get; set; }
}
