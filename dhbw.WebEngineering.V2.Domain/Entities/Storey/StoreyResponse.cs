namespace dhbw.WebEngineering.V2.Domain.Entities.Storey;

public record StoreyResponse
{
    public List<ReadStoreyDto> Storeys { get; set; } = new List<ReadStoreyDto>();

    public StoreyResponse(List<ReadStoreyDto> storeys)
    {
        Storeys = storeys;
    }
}
